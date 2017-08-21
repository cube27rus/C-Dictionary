using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2Gis
{
    public class MyDictionary<UserType, TValue> : Dictionary<UserType, TValue>
    {
        private readonly Dictionary<UserType, TValue> UsersStorage = new Dictionary<UserType, TValue>();
        private readonly object syncObj = new object();

        //добавить в UsersStorage
        public void Add(UserType key, TValue value)
        {
            var keyUser = key;
            lock (syncObj)
            {
                try
                {
                    UsersStorage.Add(keyUser, value);
                }
                catch (System.ArgumentException e)
                {

                }
            }
           
        }

        //удалить из UsersStorage
        public bool Remove(UserType key)
        {
            var keyUser = key;
            lock (syncObj)
            {
                return UsersStorage.Remove(keyUser);
            }
        }

        //получить значение из UsersStorage
        public TValue Get(UserType key)
        {
            var keyUser = key;
            lock (syncObj)
            {
                if (UsersStorage[keyUser] != null)
                    return UsersStorage[keyUser];
                else
                    return default(TValue);
            }
              
        }

        //получить UsersStorage
        public Dictionary<UserType,TValue> getDictionary()
        {
            return UsersStorage;
        }
    }   

    public class UserType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public UserType(int id, string name)
        {
            Id = id;
            Name = name;
        }

        //получить хэш юзера
        public override int GetHashCode()
        {
            return (this.Id.GetHashCode()+this.Name.GetHashCode()*Name.Length)/Id;
        }
        
        //сравнить юзеров по хэшу
        public override bool Equals(object user)
        {
            var comparedUser = user as UserType;

            if (comparedUser == null)
            {
                return false;
            }

            return (this.GetHashCode() == comparedUser.GetHashCode());
        }


    }

    class TestProgram
    {
        static void Main(string[] args)
        {

            // создаем новый обьект
            MyDictionary<UserType, string> usersDict = new MyDictionary<UserType, string>();

            // помещаем в него юзеров. Намеренно дублируем последнего
            usersDict.Add(new UserType(1, "A"), "Петров");
            usersDict.Add(new UserType(2, "B"), "Иванов");
            usersDict.Add(new UserType(3, "C"), "Сидоров");
            usersDict.Add(new UserType(3, "C"), "Сидоров");

            // выводим в консоль содержимое словаря
            foreach (KeyValuePair<UserType, string> sample in usersDict.getDictionary())
            {
                Console.WriteLine(sample.Value.ToString());
            }

            
            Console.ReadKey();

        }
    }
}
