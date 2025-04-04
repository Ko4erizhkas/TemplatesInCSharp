using TemplatesInCSharp.AbstractFabric;
using TemplatesInCSharp.Adapter;
using TemplatesInCSharp.Decorator;
using TemplatesInCSharp.FabricMethod;
using TemplatesInCSharp.Facade;
using TemplatesInCSharp.Observer;
using TemplatesInCSharp.Singleton;
using TemplatesInCSharp.SOLID;
using TemplatesInCSharp.Stradegy;
using TemplatesInCSharp.TemplateMethod;


namespace TemplatesInCSharp.Facade
{
    public class Engine
    {
        public void start() => Console.WriteLine("Engine start");
        public void stop() => Console.WriteLine("Engine stop");
    }
    public class Brake
    {
        public void apply() => Console.WriteLine("Brakes apply");
        public void release() => Console.WriteLine("Brakes release");
    }
    public class Control // Рулевое управление
    {
        public void turnLeft() => Console.WriteLine("Turn left");
        public void turnRight() => Console.WriteLine("Turn right");
    }
    public class CarFacade
    {
        private readonly Engine engine = new Engine();
        private readonly Brake brake = new Brake();
        private readonly Control control = new Control();
        public void drive()
        {
            engine.start();
            brake.apply();
            control.turnLeft();
        }
        public void stop()
        {
            engine.stop();
            brake.release();
            control.turnRight();
        }
    }
    public class CarFacadeMain
    {
        static public void FacadeMain()
        {
            CarFacade car = new CarFacade();
            Console.WriteLine("Шаблон: Фасад");
            car.drive();
            car.stop();
        }
    }
}
namespace TemplatesInCSharp.Adapter
{
    public interface ILogger
    {
        void Log(string message);
    }
    public class OldLogger
    {
        public void logMessage(string message) => Console.WriteLine($"{message}");
    }
    public class LogAdapter : ILogger
    {
        private readonly OldLogger oldLogger = new OldLogger();
        public void Log(string message) => oldLogger.logMessage(message);
    }
    public class AdapterMain
    {
        static public void AdaptMain()
        {
            Console.WriteLine("Шаблон: Адаптер");
            ILogger logger = new LogAdapter();
            logger.Log("Ёжик пукнул");
        }
    }
}
namespace TemplatesInCSharp.AbstractFabric
{
    public interface IButton
    {
        void Render();
    }
    public interface IGUIFactory
    {
        IButton CreateButton();
    }

    public class WinButton : IButton
    {
        public void Render() => Console.WriteLine("Windows button");
    }
    public class MacButton : IButton
    {
        public void Render() => Console.WriteLine("Mac button");
    }
    public class WinFactory : IGUIFactory
    {
        public IButton CreateButton() => new WinButton();
    }
    public class MacFactory : IGUIFactory
    {
        public IButton CreateButton() => new MacButton();
    }
    public class AbsFactoryMain
    {
        static public void AbsFacMain()
        {
            IGUIFactory factory = new WinFactory();
            IButton button = factory.CreateButton();
            button.Render();
        }
    }
}
namespace TemplatesInCSharp.FabricMethod
{
    #region Transport
    public abstract class Transport
    {
        public abstract void Deliver();
    }
    public class Trusk : Transport
    {
        public override void Deliver()
        {
            Console.WriteLine("Delivery with help Trusk");
        }
    }
    public class Ship : Transport
    {
        public override void Deliver()
        {
            Console.WriteLine("Delivery with help Ship");
        }
    }
    #endregion
    #region Logistic
    public abstract class Logistic
    {
        public abstract Transport CreateTransport();
    }
    public class RoadLogistic() : Logistic
    {
        public override Transport CreateTransport()
        {
            return new Trusk();
        }
    }
    public class SeaLogistic() : Logistic
    {
        public override Transport CreateTransport()
        {
            return new Ship();
        }
    }
    #endregion
    #region Main
    public class FabMethodMain
    {
        static public void FabMethMain()
        {

            Logistic logistic = new RoadLogistic();
            Transport transport = logistic.CreateTransport();
            transport.Deliver();

            logistic = new SeaLogistic();
            transport = logistic.CreateTransport();
            transport.Deliver();
        }
    }
    #endregion
}
namespace TemplatesInCSharp.Decorator
{
    public interface IPizza
    {
        double GetCost();
        string GetDesc();
    }
    public class Pizza : IPizza
    {
        public double GetCost()
        {
            return 1.0;
        }
        public string GetDesc()
        {
            return "Pizza with: ";
        }

    }
    public abstract class PizzaDecorator : IPizza
    {
        protected IPizza pizza;
        protected PizzaDecorator(IPizza pizzas) => this.pizza = pizzas;
        public abstract string GetDesc();
        public abstract double GetCost();
    }
    public class CheessDecorator : PizzaDecorator
    {
        public CheessDecorator(IPizza pizza) : base(pizza) { }
        public override string GetDesc() => pizza.GetDesc() + "Cheese";
        public override double GetCost() => pizza.GetCost() + 2.0;
    }
    public class WaterDecorator : PizzaDecorator
    {
        public WaterDecorator(IPizza pizzas) : base(pizzas) { }
        public override string GetDesc() => pizza.GetDesc() + "Water";
        public override double GetCost() => pizza.GetCost() + 813467.265;
    }
    public class DecoratorMain
    {
        static public void DecMain()
        {
            IPizza pizza = new WaterDecorator(new Pizza());
            Console.WriteLine($"{pizza.GetDesc()} {pizza.GetCost()}");
        }
    }
}
namespace TemplatesInCSharp.Observer
{
    public interface IObserver
    {
        void Update(double temp);
    }
    public class WeatherStation
    {
        private List<IObserver> observers = new List<IObserver>();
        private double temp;
        public void AddObserver(IObserver _observer)
        {
            observers.Add(_observer);
        }
        public void RemoveObserver(IObserver _observer)
        {
            observers.Remove(_observer);
        }
        public void SetTemp(double _temp)
        {
            temp = _temp;
            NotifyObserver();
        }
        private void NotifyObserver()
        {
            foreach (var obs in observers)
            {
                obs.Update(temp);
            }
        }
    }
    public class PhoneDisplay : IObserver
    {
        public void Update(double temp)
        {
            Console.WriteLine($"Телефон: Температура {temp}°C");
        }
    }
    public class WebDisplay : IObserver
    {
        public void Update(double temp)
        {
            Console.WriteLine($"Температура на улице сегодня: {temp}°C");
        }
    }

    public class ObserverMain
    {
        static public void ObsMain()
        {
            WeatherStation station = new WeatherStation();

            PhoneDisplay phone = new PhoneDisplay();
            WebDisplay web = new WebDisplay();

            station.AddObserver(phone);
            station.SetTemp(5.9);

            station.AddObserver(web);
            station.SetTemp(537);
        }
    }
}
namespace TemplatesInCSharp.Stradegy
{
    public interface IDeliveryCostCalc
    {
        double CalculateCost(Order order);
    }
    public class Order
    {
        public double Weight { get; set; }
        public double Distance { get; set; }
    }
    public class StandartDevilery : IDeliveryCostCalc
    {
        public double CalculateCost(Order order)
        {
            return 5.0;
        }
    }
    public class ExpressDevilery : IDeliveryCostCalc
    {
        public double CalculateCost(Order order)
        {
            return 10.0 + 2.0 * order.Weight;
        }
    }
    public class DronDevilery : IDeliveryCostCalc
    {
        public double CalculateCost(Order order)
        {
            return 1.0 * order.Distance;
        }
    }
    public class DeliveryService
    {
        private IDeliveryCostCalc _calc;
        public DeliveryService(IDeliveryCostCalc calc)
        {
            _calc = calc;
        }
        public double GetDeliveryCost(Order order)
        {
            return _calc.CalculateCost(order);
        }
        public void SetCalc(IDeliveryCostCalc calc)
        {
            _calc = calc;
        }
    }
    public class StradegyMain
    {
        static public void StrMain()
        {
            Order order = new Order { Weight = 32.7, Distance = 100.4 };

            DeliveryService service = new DeliveryService(new StandartDevilery());
            Console.WriteLine($"Стандартная доставка: {service.GetDeliveryCost(order)}");

            service.SetCalc(new ExpressDevilery());
            Console.WriteLine($"Экспресс доставка: {service.GetDeliveryCost(order)}");

            service.SetCalc(new DronDevilery());
            Console.WriteLine($"Доставка дроном: {service.GetDeliveryCost(order)}");
        }
    }
}
namespace TemplatesInCSharp.TemplateMethod
{
    public abstract class Game
    {
        public void Play()
        {
            Init();
            StartPlay();
            EndPlay();
        }
        public abstract void Init();
        public abstract void StartPlay();
        public abstract void EndPlay();
    }
    public class Chess : Game
    {
        public override void Init() => Console.WriteLine("Инициализация: Шахматы");
        public override void StartPlay() => Console.WriteLine("Игра началась");
        public override void EndPlay() => Console.WriteLine("Игра окончена");
    }
    public class TemplateMethodMain
    {
        static public void TempMethMain()
        {
            Chess chess = new Chess();
            chess.Play();
        }
    }
}
namespace TemplatesInCSharp.Singleton
{
    public class Logger
    {
        private static Logger? _instance;
        private static readonly object _lock = new();
        private Logger() { }
        public static Logger Instance
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new Logger();
                }
            }
        }
        public void Log(string message) => Console.WriteLine($"Log: {message}");
    }
    public class SingletonMain
    {
        static public void SingMain()
        {
            var logger = Logger.Instance;
            logger.Log("Одиночка работает");
        }
    }
}
namespace TemplatesInCSharp.SOLID
{
    #region S
    public class ProcessOrder
    {
        public void ProcessingOrder(string orderId)
        {
            Console.WriteLine($"Processing order {orderId}");
        }
    }
    public class SaveData
    {
        public void SavingToDataBase(string orderId)
        {
            Console.WriteLine($"Saving order {orderId} to database");
        }
    }
    public class SendToEmail
    {
        public void SendConfirmationEmail(string email, string orderId)
        {
            Console.WriteLine($"Sending email to {email} for order {orderId}");
        }
    }
    public class MainS
    {
        static public void SMain()
        {
            ProcessOrder processOrder = new ProcessOrder();
            SaveData saveData = new SaveData();
            SendToEmail sendToEmail = new SendToEmail();

            processOrder.ProcessingOrder("123");
            saveData.SavingToDataBase("123");
            sendToEmail.SendConfirmationEmail("sag@email.com", "123");
        }
    }
    #endregion
    #region O
    public interface IDisc
    {
        double CalcDisc();
    }
    public class CalcForRegular : IDisc
    {
        public double Amount { get; set; }
        public double CalcDisc() => (Amount > 0) ? Amount * 0.1 : 0;

    }
    public class CalcForVip : IDisc
    {
        public double Amount { get; set; }
        public double CalcDisc() => (Amount > 0) ? Amount * 0.2 : 0;
    }
    public class CalcDiscount
    {
        public double CalcDisc(IDisc disc) => disc.CalcDisc();
    }

    public class MainO
    {
        static public void OMain()
        {
            CalcDiscount calc = new CalcDiscount();
            Console.WriteLine(calc.CalcDisc(new CalcForVip { Amount = 23.234 }));
        }

    }
    #endregion
    #region L
    public interface IEngine
    {
        void StartEngine();
    }
    public interface IWithoutEngine
    {
        void Move();
    }

    public class Vehicle : IEngine
    {
        public void StartEngine() => Console.WriteLine("Engine start");
    }
    public class Bycycle : IWithoutEngine
    {
        public void Move() => Console.WriteLine("Run on bicycle");
    }
    public class TransportManager
    {
        public void StartEngine(IEngine engine) => engine.StartEngine();
        public void Move(IWithoutEngine without) => without.Move();
    }
    public class MainL
    {
        static public void LMain()
        {
            TransportManager transport = new TransportManager();

            transport.StartEngine(new Vehicle());
            transport.Move(new Bycycle());
        }
    }
    #endregion
    #region I
    public interface IPrint
    {
        void Print(string doc);
    }
    public interface IScan
    {
        void Scan(string doc);
    }
    public interface IFax
    {
        void Fax(string doc);
    }
    public class OldPrinter : IPrint
    {
        public void Print(string doc) => Console.WriteLine($"Printing: {doc}");
    }
    public class OldScaner : IScan
    {
        public void Scan(string doc) => Console.WriteLine($"Scaning: {doc}");
    }
    public class OldFax : IFax
    {
        public void Fax(string doc) => Console.WriteLine($"Faxing: {doc}");
    }
    public class MainI
    {
        static public void IMain()
        {
            IPrint printer = new OldPrinter();
            IScan scanner = new OldScaner();
            IFax fax = new OldFax();

            printer.Print("123");
            scanner.Scan("123");
            fax.Fax("123");
        }
    }
    #endregion
    #region D
    public interface ILog
    {
        void Log(string message);
    }
    public class FileLogger : ILog
    {
        public void Log(string message) => Console.WriteLine("Logging to file " + message);
    }
    public class ErrorReporter : ILog
    {
        public void Log(string message) => Console.WriteLine("Error report " + message);
    }
    public class LogManager
    {
        private readonly ILog log;
        public LogManager(ILog _log) { log = _log; }
        public void LogMessage(string message) { log.Log(message); }
    }

    public class MainD
    {
        static public void DMain()
        {
            LogManager log = new LogManager(new FileLogger());
            log.LogMessage("123");

            log = new LogManager(new ErrorReporter());
            log.LogMessage("1234");
        }
    }
    #endregion
}
namespace TemplatesInCSharp.Mains
{
    internal class Program
    {
        private static void Main(string[] args)
        {

            CarFacadeMain.FacadeMain();
            Console.WriteLine();

            AdapterMain.AdaptMain();
            Console.WriteLine();

            AbsFactoryMain.AbsFacMain();
            Console.WriteLine();

            FabMethodMain.FabMethMain();
            Console.WriteLine();

            DecoratorMain.DecMain();
            Console.WriteLine();

            ObserverMain.ObsMain();
            Console.WriteLine();

            StradegyMain.StrMain();
            Console.WriteLine();

            TemplateMethodMain.TempMethMain();
            Console.WriteLine();

            SingletonMain.SingMain();
            Console.WriteLine();

            Console.WriteLine("SOLID");

            Console.WriteLine("S");
            MainS.SMain();

            Console.WriteLine("O");
            MainO.OMain();

            Console.WriteLine("L");
            MainL.LMain();

            Console.WriteLine("I");
            MainI.IMain();

            Console.WriteLine("D");
            MainD.DMain();
        }
    }
}