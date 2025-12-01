using EducaOnline.Core.Messages;
using EducaOnline.Core.Messages.CommonMessages.DomainEvents;
using EducaOnline.Core.Messages.CommonMessages.Notifications;
using FluentValidation.Results;

namespace EducaOnline.Core.Communication
{
    public interface IMediatorHandler
    {
        Task PublicarEvento<T>(T evento) where T : Event;        
        Task PublicarNotificacao<T>(T notificacao) where T : DomainNotification;
        Task PublicarDomainEvent<T>(T notificacao) where T : DomainEvent;
        Task<ValidationResult> EnviarComando<T>(T comando) where T : Command;
    }
}
