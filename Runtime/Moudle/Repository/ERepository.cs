using System;
using System.Collections.Generic;

namespace EasyGamePlay
{
    public class ERepository
    {
        private Dictionary<Type, IRepository> repositorys= new Dictionary<Type, IRepository>();

        public void Regist(IRepository repository) 
        {
            Type type = repository.GetType();
            if(!repositorys.ContainsKey(type))
            {
                repositorys.Add(type,repository);
            }
        }

        public void UnRegist<T>() where T: class,IRepository
        {
            repositorys.Remove(typeof(T));
        }

        public T GetRepository<T>() where T : class,IRepository
        {
            repositorys.TryGetValue(typeof(T), out IRepository repository);
            return repository as T;
        }

        public void Command<T>() where T : struct, ICommand
        {
            T command = new T();
            command.Init(repositorys);
            command.Command();
        }

        public T Request<T>(IRequest<T> request)
        {
            request.Init(repositorys);
            return request.Request();
        }
    }
}
