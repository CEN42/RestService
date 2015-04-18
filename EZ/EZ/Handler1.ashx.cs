using JsonServices;
using JsonServices.Web;


namespace EZ
{
    public class Handler1 : JsonHandler
    {
        public Handler1()
        {
            this.service.Name = "EZ";
            this.service.Description = "EZ snips for android appliation";
            InterfaceConfiguration IConfig = new InterfaceConfiguration("RestAPI" , typeof(IServiceAPI), typeof(ServiceAPI));
            this.service.Interfaces.Add(IConfig);
        }

    }
}