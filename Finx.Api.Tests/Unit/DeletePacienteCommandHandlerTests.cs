using System;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Finx.Api.Handlers;
using Finx.Domain;

namespace Finx.Api.Tests.Unit
{
    public class DeletePacienteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Delete_Existing_Paciente_And_Return_True()
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
            mockRepo.Setup(r => r.DeleteAsync(existingPaciente.Id)).Returns(Task.CompletedTask);

            var handler = new DeletePacienteCommandHandler(mockRepo.Object);
            var command = new DeletePacienteCommand(existingPaciente.Id);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            mockRepo.Verify(r => r.GetByIdAsync(existingPaciente.Id), Times.Once);
            mockRepo.Verify(r => r.DeleteAsync(existingPaciente.Id), Times.Once);
        }

        [Fact]
        public async Task Handle_Should_Return_False_When_Paciente_Not_Found()
        {
            // Arrange
            var mockRepo = new Mock<IPacienteRepository>();
            var nonExistentId = Guid.NewGuid();
            mockRepo.Setup(r => r.GetByIdAsync(nonExistentId)).ReturnsAsync((Paciente?)null);

            var handler = new DeletePacienteCommandHandler(mockRepo.Object);
            var command = new DeletePacienteCommand(nonExistentId);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            mockRepo.Verify(r => r.GetByIdAsync(nonExistentId), Times.Once);
            mockRepo.Verify(r => r.DeleteAsync(It.IsAny<Guid>()), Times.Never);
        }
    }
}
