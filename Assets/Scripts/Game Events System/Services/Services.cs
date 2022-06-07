using UnityEngine;

public class Services : IServiceLocator
{
    private static Services _instance;
    public static IServiceLocator Container => _instance ??= new Services();

    public void RegisterSingle<TService>(TService implementation) where TService : IService =>
      Implementation<TService>.ServiceInstance = implementation;

    public TService Single<TService>() where TService : IService =>
      Implementation<TService>.ServiceInstance;

    private class Implementation<TService> where TService : IService
    {
        public static TService ServiceInstance;
    }
}
