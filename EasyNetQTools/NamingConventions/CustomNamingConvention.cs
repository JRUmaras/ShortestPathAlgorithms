using System;
using EasyNetQ;
using EasyNetQ.DI;
using EasyNetQTools.NamingConventions.Models;

namespace EasyNetQTools.NamingConventions;

public class CustomNamingConvention : Conventions
{
    public CustomNamingConvention(ITypeNameSerializer typeNameSerializer, CustomNaming naming) 
        : base(typeNameSerializer)
    {
        RpcRequestExchangeNamingConvention = _ => naming.ExchangeName;
        RpcResponseExchangeNamingConvention = _ => naming.ExchangeName;
        //RpcReturnQueueNamingConvention = _ => "put-something-unique-here"
        RpcRoutingKeyNamingConvention = _ => naming.RequestQueueName;
    }
    
    public static Action<IServiceRegister> CreateRegistrationAction(CustomNaming customNaming) =>
        services => services.Register<IConventions>(serviceResolver => 
            new CustomNamingConvention(serviceResolver.Resolve<ITypeNameSerializer>(), customNaming));
}