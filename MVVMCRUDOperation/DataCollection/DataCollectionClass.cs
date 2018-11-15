using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MVVMCRUDOperation.Model;

namespace MVVMCRUDOperation.DataCollection
{
    static class DataCollectionClass
    {

       public static ObservableCollection<Student> ListStudent()
        {
            return new ObservableCollection<Student>()
            {
                new Student(1, "liza" , "Australia", "2001", "Melbourne", "4220", "354648-4657" ,"../Assets/eliza.JPG" ),
                new Student(2, "daniel" ,"UK", "2004", "London", "4567", "354648-5637" ,"../Assets/daniel.JPG" ),
                new Student(3, "benny" , "USA", "2001", "New York", "5673", "354648-4567" ,"../Assets/benny.JPG" ),
                new Student(4, "ann" , "Denmark", "2000", "Copenhagen", "5992", "354648-4657" ,"../Assets/ann.JPG" ),
                new Student(5, "carol" , "China", "2000", "Beijing", "5992", "354648-4657" ,"../Assets/carol.JPG" )
            };
        }
    }
}
