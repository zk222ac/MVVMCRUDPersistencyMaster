using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using MVVMCRUDOperation.DataCollection;
using MVVMCRUDOperation.Model;
using MVVMCRUDOperation.Persistency;
using MVVMCRUDOperation.View;

namespace MVVMCRUDOperation.ViewModel
{
    class StudentVm : NotifyPropertyChangeClass
    {
        // This property mean Listview by default select selected first item of Listview 
        public int SelectedIndex { get; } = 0; // 0 index

        // Step 1 : View ListView Item Source attribute connect this property which we also called source property (ListStudents)
       
        public ObservableCollection<Student> ListStudents { get; set; }

        // Step 3: Selected Item source instance field
        private Student _selectedItemStudent;

        // Step 4 : Add new Student
        public Student AddStudent { get; set; }

        // Step 3a: Selected Item source property , On per student item selection change index
        // we need to call PropertyChange interface because we change model properties in every selection
        public Student SelectedItemStudent
        {
            get => _selectedItemStudent;
            set
            {
                _selectedItemStudent = value;
                OnPropertyChanged(nameof(SelectedItemStudent));
            }
        }
        // Step 3b: Create different Source Commands which is exposed delegate methods 
        public RelayCommand AddStudentCommand { get; set; }
        public RelayCommand UpdateStudentCommand { get; set; }
        public RelayCommand DeleteStudentCommand { get; set; }
        public RelayCommand RefreshStudentCommand { get; set; }
        public ICommand SearchStudentCommand { get; set; }

        public RelayCommand GoPage1Command { get; set; }

        // File Persistency 
        private  FilePersistency<Student> _filePersistency;
       

        //  Frame Navigation property
        public readonly FrameNavigate FrameNavigate;
        // User singleton property 
        public readonly Singleton UserSingleton;

        // Search List Observable collection
        public ObservableCollection<Student> SearchListStudent;
        public StudentVm()
        {
            // Data Persistency 
            _filePersistency = new FilePersistency<Student>();

            // Step 2: Add some by default values , either Load data from Persistency
            //ListStudents = DataCollectionClass.ListStudent();
             RunAsyncLoadData();
            
            // initialize the selectedItemStudent property in constructor
            SelectedItemStudent = new Student();
            AddStudent = new Student();
            
            // Relay Command 
            AddStudentCommand = new RelayCommand(DoAddStudent);
            UpdateStudentCommand = new RelayCommand(DoUpdateStudent);
            DeleteStudentCommand = new RelayCommand(DoDeleteStudent);
            RefreshStudentCommand = new RelayCommand(DoRefreshStudent);
            // remember here Passing delegate method as a object parameter
            SearchStudentCommand = new RelayCommandArg(DoSearchStudent);

            GoPage1Command = new RelayCommand(DoGoPage1);
           
            // Search List Student
            SearchListStudent = new ObservableCollection<Student>();

            // Frame navigation Object initialization 
            FrameNavigate = new FrameNavigate();

            //  User singleton Instance
            UserSingleton = Singleton.GetInstance();

           
        }

        // Command Delegates Methods
        public async void DoAddStudent()
        {
            // add the name of image in URL path
            string img = "../Assets/" + AddStudent.ImageUrl + ".jpg";
            AddStudent.ImageUrl = img;
            ListStudents.Add(AddStudent);
            await SaveAsyncMethod(ListStudents);
        }


        public void DoUpdateStudent()
        {
           foreach (var lis in ListStudents)
            {
                if (lis.Id == SelectedItemStudent.Id)
                {
                    lis.Name = SelectedItemStudent.Name;
                    lis.City = SelectedItemStudent.City;
                    lis.Cpr = SelectedItemStudent.Cpr;
                    lis.Dob = SelectedItemStudent.Dob;
                    lis.ImageUrl = SelectedItemStudent.ImageUrl;
                    lis.ZipCode = SelectedItemStudent.ZipCode;
                    lis.Country = SelectedItemStudent.ZipCode;
                }
              
            }
            
        }
        public void DoDeleteStudent()
        {
            ListStudents.Remove(SelectedItemStudent);
        }
        public void DoRefreshStudent()
        {
            // Refresh List Again
            ListStudents = DataCollectionClass.ListStudent();
            
            // Here I am using Property change Interface because it is changes the model property here
            OnPropertyChanged(nameof(ListStudents));
        }

        // Search Student By Name 
        // remember this time we pass delegate method as a object 
        public void DoSearchStudent(object obj)
        {
            try
            {
                string name = obj as string;
                if (ListStudents != null)
                {
                    foreach (var stu in ListStudents)
                    {
                        if (stu.Name.ToUpper() == name?.ToUpper())
                        {
                            SearchListStudent = new ObservableCollection<Student>
                            {
                                new Student(stu.Id, stu.Name, stu.Country, stu.Dob, stu.City, stu.ZipCode, stu.Cpr,
                                    stu.ImageUrl)
                            };

                        }
                    }

                    // we check if search is found then assign this search list to ListStudent otherwise make a null to this ListStudent
                    if (SearchListStudent != null)
                    {
                        ListStudents = SearchListStudent;
                        OnPropertyChanged(nameof(ListStudents));
                    }
                    else
                    {
                        ListStudents.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.Write(e);
            }
        }
        public void DoGoPage1()
        {
            // save complete student object instance in singleton SetStudent method
            UserSingleton.SetStudent(SelectedItemStudent);

            // Redirect MainPage to Page1
            Type type = typeof(Page1);
            FrameNavigate.ActivateFrameNavigation(type);
        }

        // Data Persistency Methods ................................................................. 

        public async void RunAsyncLoadData()
        {
            ListStudents = await _filePersistency.LoadAsync();
        }
        // Load data from file
        public  async Task<ObservableCollection<Student>> LoadAsyncMethod()
        {
           return  await _filePersistency.LoadAsync();
        }

        // Save data into file 
        public  async Task SaveAsyncMethod(ObservableCollection<Student> students)
        {
            await _filePersistency.SaveAsync(students);
        }

       
    }
}
