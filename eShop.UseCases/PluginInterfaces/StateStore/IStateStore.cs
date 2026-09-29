using System;

namespace eShop.UseCases.PluginInterfaces.StateStore
{
    public interface IStateStore
    {
        void AddStateChangeListeners(Action listeners);
        void RemoveStateChangeListeners(Action listeners);
        void BroadCastStateChange();
    }
}
