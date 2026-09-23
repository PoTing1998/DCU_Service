namespace ASI.Wanda.DMD.Service.Handlers
{
    public interface IDMDMessageHandler
    {
        void Handle(ASI.Wanda.DMD.Message.Message message, DMDHelper helper);
    }
}
