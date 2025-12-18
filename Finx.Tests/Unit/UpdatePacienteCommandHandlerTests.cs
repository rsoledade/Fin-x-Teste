using Moq;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using MediatRUnit = MediatR.Unit;
using Finx.Application.Handlers.Pacientes.Commands;

namespace Finx.Api.Tests.Unit
{
    public class UpdatePacienteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Update_Existing_Paciente_And_Return_Unit()
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
            var command = new UpdatePacienteCommand(existingPaciente.Id, "Joao Santos", DateTime.UtcNow.AddYears(-31), "(11) 88888-8888");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(MediatRUnit.Value, result);
            mockRepo.Verify(r => r.GetByIdAsync(existingPaciente.Id), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Paciente>()), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_Unit_When_Paciente_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IPacienteRepository>();
            var nonExistentId = Guid.NewGuid();
            mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((Paciente?)null);

            var handler = new UpdatePacienteCommandHandler(mockRepo.Object);
            var command = new UpdatePacienteCommand(nonExistentId, "Joao Santos", DateTime.UtcNow.AddYears(-31), "(11) 88888-8888");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(MediatRUnit.Value, result);
            mockRepo.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once);
            mockRepo.Verify(r => r.UpdateAsync(It.IsAny<Paciente>()), Times.Never);
        }
    }
}
