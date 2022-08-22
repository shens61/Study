using System;

namespace AutoMapper.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var config = new MapperConfiguration(cfg => cfg.CreateMap<Employee, EmployeeDTO>());
            var mapper = config.CreateMapper();
            config.AssertConfigurationIsValid();

            Employee emp = new Employee
            {
                UserID = "shens61",
                Name = "shicheng shen",
                Age = 30
            };

            var empDTO = mapper.Map<EmployeeDTO>(emp);          

            string emplyee = ObjectDumper.Dump(empDTO, DumpStyle.Console);
            Console.WriteLine(emplyee);
            Console.ReadKey(true);
        }
    }

    class Employee
    {
        public string UserID { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class EmployeeDTO
    {
        public string UserID { get; set; }
        public string Name { get; set; }
    }
}
