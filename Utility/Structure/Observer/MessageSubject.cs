namespace Utilities.Observer
{
    public class MessageSubject : ISubject
    {
        private List<IObserver> observers = new List<IObserver>();

        public void RegisterObserver(IObserver observer)
        {
            observers.Add(observer);
        }

        public void RemoveObserver(IObserver observer)
        {
            observers.Remove(observer);
        }

        public void NotifyObservers(object message)
        {
            foreach (var observer in observers)
            {
                observer.Update(message);
            }
        }
    }
}
