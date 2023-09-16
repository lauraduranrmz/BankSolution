using System.Text.RegularExpressions;
using BankConsole;

if(args.Length == 0)
  EmailService.SendMail();
else
   ShowMenu();

void ShowMenu()
{
    Console.Clear();
    Console.WriteLine("Selecciona una opción: ");
    Console.WriteLine("1 - Crear un Usuario nuevo. ");
    Console.WriteLine("2 - Eliminar un Usuario existente.");
    Console.WriteLine("3 - Salir.");

    int option = 0;
    do
    {
        string input = Console.ReadLine();
        if(!int.TryParse(input, out option))
            Console.WriteLine("Debes ingresar un número (1, 2 o 3).");
        else if(option>3)
            Console.WriteLine("Debes ingresar un número válido (1, 2, o 3).");
    }
    while(option == 0 || option>3);

    switch (option)
    {
        case 1:
            CreateUser();
            break;
        case 2:
            DeleteUser();
            break;
        case 3:
            Environment.Exit(0);
            break;
    }
}

void CreateUser()
{
    Console.Clear();
    Console.WriteLine("Ingresa la información del usuario: ");
    Console.Write("ID: ");
    int ID;
    do
    {
        string input = Console.ReadLine();
        if(!int.TryParse(input, out ID) || ID<=0)
            Console.Write("Debes ingresar un ID positivo, ingresalo nuevamente: ");
        else if(Storage.IsUserIDTaken(ID))
            Console.Write($"El ID {ID} ya esta utilizado, ingresa otro: ");
    }while(ID <= 0 || Storage.IsUserIDTaken(ID));
    
    Console.Write("Nombre: ");
    string name = Console.ReadLine();
    
    string email;
    do
    {
        Console.Write("Email: ");
        email = Console.ReadLine();

        if(!validateEmail(email))
        {
            Console.WriteLine("El formato del email es incorrecto. Ingrese una dirección valida: ");
        }
        else
        {
            break;
        }
    }while(true);
    
    decimal balance;
    do
    {
        Console.Write("Saldo: ");
        balance = decimal.Parse(Console.ReadLine());
         if(balance < 0)
         {
            Console.WriteLine("El saldo debe ser una cantidad postiva. Ingresalo nuevamente.");
         }
         else
         {
            break;
         }
    }while(true);

    char userType;
    do
    {
        Console.Write("Escribe 'c' si el usuario es Cliente, 'e' si es Empleado: ");
        userType = char.Parse(Console.ReadLine());

        if(userType !='c' && userType !='e')
        {
             Console.WriteLine("Solo se permiten los caracteres 'c' y 'e'. Ingresalo nuevamente.");
        }
        else
        {
            break;
        }
    }while(true);

    

    User newUser;

    if(userType.Equals('c'))
    {
        Console.Write("Regimen Fiscal: ");
        char taxRegime = char.Parse(Console.ReadLine());

        newUser = new Client(ID, name, email, balance, taxRegime);
    }

    else
    {
        Console.Write("Departamento: ");
        string department = Console.ReadLine();
        newUser = new Employee(ID, name, email, balance, department);
    }

    Storage.AddUser(newUser);
    Console.WriteLine("Usuario creado.");
    Thread.Sleep(2000);
    ShowMenu();
}

void DeleteUser()
{
    Console.Clear();
    int ID;
    bool idValid = false;
    do
    {
        Console.Write("Ingresa el ID del usuario a eliminar: ");
        if(int.TryParse(Console.ReadLine(), out ID) && ID > 0)
        {
            if(!Storage.IsUserIDTaken(ID))
            {
                Console.WriteLine($"El ID {ID} no existe, ingresa otro.");
            }
            else
            {
                idValid = true;
            }
        }
        else
        {
           Console.WriteLine("El ID debe ser un numero entero positivo. Ingresalo nuevamente");
        }
            
    }while(!idValid);

    string result = Storage.DeleteUser(ID);

    if (result.Equals("Success"))
    {
        Console.Write("Usuario eliminado.");
        Thread.Sleep(2000); //tiempo espera 2 SEGUNDOS
        ShowMenu();

    }
}

bool validateEmail(string email)
{
    string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z]+\.[a-zA-Z]{2,}$";
    return Regex.IsMatch(email, pattern);
}
