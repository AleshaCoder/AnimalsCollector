public interface IServiceLocator
{
    void RegisterSingle<TService>(TService implementation) where TService : IService;
    TService Single<TService>() where TService : IService;
}