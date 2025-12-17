using Moq;
using Finx.Api.Validators;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using Finx.Api.Handlers.Pacientes.Commands;

namespace Finx.Api.Tests.Unit
{
    public class CreatePacienteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Call_AddAsync_And_Return_Id()
        {
            // Arrange
            var mockRepo = new Mock<IPacienteRepository>();
            var expectedId = Guid.NewGuid();
            mockRepo.Setup(r => r.AddAsync(It.IsAny<Paciente>())).ReturnsAsync(expectedId);

            var handler = new CreatePacienteCommandHandler(mockRepo.Object);
            var command = new CreatePacienteCommand("Joao Silva", "12345678909", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(expectedId, result);
            mockRepo.Verify(r => r.AddAsync(It.IsAny<Paciente>()), Times.Once);
        }

        [Fact]
        public void Validator_Should_Reject_Invalid_CPF()
        {
            // Arrange
            var validator = new CreatePacienteCommandValidator();
            var command = new CreatePacienteCommand("Joao Silva", "12345678900", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

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
            var validator = new CreatePacienteCommandValidator();
            var command = new CreatePacienteCommand("Joao Silva", "52998224725", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.True(result.IsValid);
        }

        [Fact]
        public void Validator_Should_Reject_Empty_CPF()
        {
            // Arrange
            var validator = new CreatePacienteCommandValidator();
            var command = new CreatePacienteCommand("Joao Silva", "", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf");
        }

        [Fact]
        public void Validator_Should_Reject_CPF_With_All_Same_Digits()
        {
            // Arrange
            var validator = new CreatePacienteCommandValidator();
            var command = new CreatePacienteCommand("Joao Silva", "11111111111", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Cpf" && e.ErrorMessage == "CPF inválido");
        }

        [Fact]
        public void Validator_Should_Reject_Future_Birth_Date()
        {
            // Arrange
            var validator = new CreatePacienteCommandValidator();
            var command = new CreatePacienteCommand("Joao Silva", "52998224725", DateTime.UtcNow.AddYears(1), "(11) 99999-9999");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "DataNascimento" && e.ErrorMessage == "Data de nascimento inválida");
        }

        [Fact]
        public void Validator_Should_Reject_Empty_Nome()
        {
            // Arrange
            var validator = new CreatePacienteCommandValidator();
            var command = new CreatePacienteCommand("", "52998224725", DateTime.UtcNow.AddYears(-30), "(11) 99999-9999");

            // Act
            var result = validator.Validate(command);

            // Assert
            Assert.False(result.IsValid);
            Assert.Contains(result.Errors, e => e.PropertyName == "Nome");
        }
    }
}
