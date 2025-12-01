using System.Threading;
using System.Threading.Tasks;
using MediatR;
using EducaOnline.Aluno.API.Application.Events;

namespace EducaOnline.Aluno.API.Application.EventHandlers
{
    public class CertificadoEmitidoEventHandler : INotificationHandler<CertificadoEmitidoEvent>
    {
        public Task Handle(CertificadoEmitidoEvent notification, CancellationToken cancellationToken)
        {
            // talvez enviar o certificado por email
            return Task.CompletedTask;
        }
    }
}
