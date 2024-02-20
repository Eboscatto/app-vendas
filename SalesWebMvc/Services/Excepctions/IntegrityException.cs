using System;

// Exceção personalizada de serviço para erros de integridade referencial
namespace SalesWebMvc.Services.Excepctions
{
    public class IntegrityException : ApplicationException
    {
        public IntegrityException(string message) : base(message)
        {

        }
    }
}
