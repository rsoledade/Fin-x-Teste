using Moq;
using Finx.Api.Validators;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using Finx.Api.Handlers.Pacientes.Commands;

namespace Finx.Api.Tests.Unit
{
    public class UpdatePacienteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Update_Existing_Paciente_And_Return_True()
        {
            // Arrange
            var mockRepo = new Mock<IPacienteRepository>();
            var existingPaciente = new Paciente
            {
                Id = Guid.NewGuid(),
                Nome = "Joao Silva",
                Cpf = "52998224725",
                DataNascimento = DateTime.UtcNow.AddYears(-30),
                DataCadastro = DateTime.UtcNow,
                Contato = "(11) 99999-9999"
            };

            mockRepo.Setup(r => r.GetByIdAsync(existingPaciente.Id)).ReturnsAsync(existingPaciente);
            mockRepo.Setup(r => r.UpdateAsync(It.IsAny<Paciente>())).Returns(Task.CompletedTask);

            var handler = new UpdatePacienteCommandHandler(mockRepo.Object);
            var command = new UpdatePacienteCommand(existingPaciente.Id, "Joao Santos", "52998224725", DateTime.UtcNow.AddYears(-31), "(11) 88888-8888");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.GetByIdAsync(existingPaciente.Id), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Paciente>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Paciente_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IPacienteRepository>();
            var nonExistentId = Guid.NewGuid();
            mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((Paciente?)null);

            var handler = new UpdatePacienteCommandHandler(mockRepo.Object);
            var command = new UpdatePacienteCommand(nonExistentId, "Joao Santos", "52998224725", DateTime.UtcNow.AddYears(-31), "(11) 88888-8888");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            mockRepo.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Paciente>()), Times.Never);
        }

        [Fact]
        public void Validator_Should_Reject_Invalid_CPF()
        {
            // Arrange
            var validator = new UpdatePacienteCommandValidator();
            var command = new UpdatePacienteCommand(Guid.NewGuid(), "Joao Silva", "12345678900", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf" && e.ErrorMessage == "CPF inválido");
        }

        [Fact]
        public void Validator_Should_Accept_Valid_CPF()
        {
            // Arrange
            var validator = new UpdatePacienteCommandValidator();
            var command = new UpdatePacienteCommand(Guid.NewGuid(), "Joao Silva", "52998224725", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }
    }
}
