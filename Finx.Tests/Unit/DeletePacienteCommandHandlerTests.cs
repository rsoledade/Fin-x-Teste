using Moq;
using Finx.Domain.Entities;
using Finx.Domain.Repositories;
using MediatRUnit = MediatR.Unit;
using Finx.Application.Handlers.Pacientes.Commands;

namespace Finx.Api.Tests.Unit
{
    public class DeletePacienteCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Delete_Existing_Paciente_And_Return_Unit()
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

            mockRepo.Setup(r => r.DeleteAsync(existingPaciente.Id)).Returns(Task.CompletedTask);

            var handler = new DeletePacienteCommandHandler(mockRepo.Object);
            var command = new DeletePacienteCommand(existingPaciente.Id);

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(MediatRUnit.Value, result);
            mockRepo.Verify(r => r.DeleteAsync(existingPaciente.Id), Times.Once);
        }
    }
}
