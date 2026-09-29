using eShop.UseCases.PluginInterfaces.StateStore;

namespace eShop.StateStore.DI
{
    public class StateStoreBase : IStateStore
    {
        protected Action? listeners;

        public void AddStateChangeListeners(Action listeners)
        {
            this.listeners += listeners;
        }

        public void RemoveStateChangeListeners(Action listeners)
        {
            this.listeners -= listeners;
        }

        public void BroadCastStateChange()
        {
            if (this.listeners != null)
                this.listeners.Invoke();
        }
    }
}
