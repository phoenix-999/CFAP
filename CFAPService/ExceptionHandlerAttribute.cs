using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel.Dispatcher;
using System.ServiceModel.Channels;
using NLog;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Collections.ObjectModel;

namespace CFAPService
{
    [AttributeUsage(AttributeTargets.Class)]
    class ExceptionHandlerAttribute : Attribute, IErrorHandler, IServiceBehavior
    {
        protected static readonly Logger Log = LogManager.GetCurrentClassLogger();

        void IServiceBehavior.AddBindingParameters(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase, Collection<ServiceEndpoint> endpoints, BindingParameterCollection bindingParameters)
        {

        }

        void IServiceBehavior.ApplyDispatchBehavior(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {
            foreach (ChannelDispatcher dispatcher in serviceHostBase.ChannelDispatchers)
            {
                dispatcher.ErrorHandlers.Add(this);
            }
        }

        bool IErrorHandler.HandleError(Exception error)
        {
            Log.Error(string.Format("Непредвиденное исключения. Время: {0}, детали:\n{1}", DateTime.Now, error.ToString()));
            return true;
        }

        void IErrorHandler.ProvideFault(Exception error, MessageVersion version, ref Message fault)
        {
            if (error is FaultException)
            {
                return;
            }

            FaultException faultException = new FaultException();
            MessageFault messageFault = faultException.CreateMessageFault();
            fault = Message.CreateMessage(version, messageFault, faultException.Action);
        }

        void IServiceBehavior.Validate(ServiceDescription serviceDescription, ServiceHostBase serviceHostBase)
        {

        }
    }
}
