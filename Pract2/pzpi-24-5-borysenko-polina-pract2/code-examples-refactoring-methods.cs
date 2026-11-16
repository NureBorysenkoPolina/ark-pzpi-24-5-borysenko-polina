using System.Collections.ObjectModel;
using EncapsulateCollectionExample.After;
using ParameterObjectExample.After;

namespace ExtractClassExample.Before
{
    public class Order
    {
        private string _street;
        private string _city;
        private string _postalCode;

        public Order(string street, string city, string postalCode)
        {
            _street = street;
            _city = city;
            _postalCode = postalCode;
        }

        public string FormatDeliveryAddress()
        {
            return $"{_street}, {_city}, {_postalCode}";
        }

        public void ChangeDeliveryAddress(
            string street,
            string city,
            string postalCode)
        {
            _street = street;
            _city = city;
            _postalCode = postalCode;
        }
    }
}

namespace ExtractClassExample.After
{
    public class DeliveryAddress
    {
        public string Street { get; }
        public string City { get; }
        public string PostalCode { get; }

        public DeliveryAddress(string street, string city, string postalCode)
        {
            Street = street;
            City = city;
            PostalCode = postalCode;
        }

        public string Format()
        {
            return $"{Street}, {City}, {PostalCode}";
        }
    }

    public class Order
    {
        public DeliveryAddress DeliveryAddress { get; private set; }

        public Order(DeliveryAddress deliveryAddress)
        {
            DeliveryAddress = deliveryAddress;
        }

        public string FormatDeliveryAddress()
        {
            return DeliveryAddress.Format();
        }

        public void ChangeDeliveryAddress(DeliveryAddress address)
        {
            DeliveryAddress = address;
        }
    }
}

namespace ParameterObjectExample.Before
{
    public class Order
    {
        public DateTime CreatedAt { get; }
        public decimal Total { get; }
        public string Status { get; }

        public Order(DateTime createdAt, decimal total, string status)
        {
            CreatedAt = createdAt;
            Total = total;
            Status = status;
        }
    }

    public class OrderService
    {
        private readonly List<Order> _orders;

        public OrderService(IEnumerable<Order> orders)
        {
            ArgumentNullException.ThrowIfNull(orders);
            _orders = orders.ToList();
        }

        public List<Order> FindOrders(
            DateTime from,
            DateTime to,
            decimal minTotal,
            decimal maxTotal,
            string status)
        {
            if (from > to)
            {
                throw new ArgumentException(
                    "Початкова дата не може бути пізніше кінцевої.");
            }

            if (minTotal > maxTotal)
            {
                throw new ArgumentException(
                    "Мінімальна сума не може бути більшою за максимальну.");
            }

            if (string.IsNullOrWhiteSpace(status))
            {
                throw new ArgumentException(
                    "Статус має бути заданий.",
                    nameof(status));
            }

            return _orders
                .Where(order =>
                    order.CreatedAt >= from &&
                    order.CreatedAt <= to &&
                    order.Total >= minTotal &&
                    order.Total <= maxTotal &&
                    string.Equals(
                        order.Status,
                        status,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

namespace ParameterObjectExample.After
{
    public class Order
    {
        public DateTime CreatedAt { get; }
        public decimal Total { get; }
        public string Status { get; }

        public Order(DateTime createdAt, decimal total, string status)
        {
            CreatedAt = createdAt;
            Total = total;
            Status = status;
        }
    }

    public class OrderFilter
    {
        public DateTime From { get; }
        public DateTime To { get; }
        public decimal MinTotal { get; }
        public decimal MaxTotal { get; }
        public string Status { get; }

        public OrderFilter(
            DateTime from,
            DateTime to,
            decimal minTotal,
            decimal maxTotal,
            string status)
        {
            From = from;
            To = to;
            MinTotal = minTotal;
            MaxTotal = maxTotal;
            Status = status;
        }
    }

    public class OrderService
    {
        private readonly List<Order> _orders;

        public OrderService(IEnumerable<Order> orders)
        {
            ArgumentNullException.ThrowIfNull(orders);
            _orders = orders.ToList();
        }

        public List<Order> FindOrders(OrderFilter filter)
        {
            ArgumentNullException.ThrowIfNull(filter);

            if (filter.From > filter.To)
            {
                throw new ArgumentException(
                    "Початкова дата не може бути пізніше кінцевої.");
            }

            if (filter.MinTotal > filter.MaxTotal)
            {
                throw new ArgumentException(
                    "Мінімальна сума не може бути більшою за максимальну.");
            }

            if (string.IsNullOrWhiteSpace(filter.Status))
            {
                throw new ArgumentException(
                    "Статус має бути заданий.",
                    nameof(filter));
            }

            return _orders
                .Where(order =>
                    order.CreatedAt >= filter.From &&
                    order.CreatedAt <= filter.To &&
                    order.Total >= filter.MinTotal &&
                    order.Total <= filter.MaxTotal &&
                    string.Equals(
                        order.Status,
                        filter.Status,
                        StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}

namespace EncapsulateCollectionExample.Before
{
    public class Employee
    {
        public int Id { get; }
        public string Name { get; }

        public Employee(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Team
    {
        public List<Employee> Employees { get; } = new();
    }
}

namespace EncapsulateCollectionExample.After
{
    public class Employee
    {
        public int Id { get; }
        public string Name { get; }

        public Employee(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }

    public class Team
    {
        private readonly List<Employee> _employees = new();
        private readonly ReadOnlyCollection<Employee> _employeesView;

        public Team()
        {
            _employeesView = _employees.AsReadOnly();
        }

        public IReadOnlyList<Employee> Employees => _employeesView;

        public void AddEmployee(Employee employee)
        {
            ArgumentNullException.ThrowIfNull(employee);

            if (_employees.Any(existing => existing.Id == employee.Id))
            {
                throw new InvalidOperationException(
                    "Працівник із таким ідентифікатором уже є в команді.");
            }

            _employees.Add(employee);
        }

        public bool RemoveEmployee(int employeeId)
        {
            var employee = _employees.FirstOrDefault(
                existing => existing.Id == employeeId);

            if (employee is null)
            {
                return false;
            }

            return _employees.Remove(employee);
        }
    }
}

public class Program
{
    public static void Main()
    {
        var orders = new List<Order>
        {
            new(new DateTime(2026, 3, 5), 1200m, "Paid"),
            new(new DateTime(2026, 3, 12), 800m, "Pending"),
            new(new DateTime(2026, 3, 20), 2500m, "Paid"),
            new(new DateTime(2026, 3, 25), 1500m, "Paid")
        };

        var service =
            new OrderService(orders);

        var filter = new OrderFilter(
            new DateTime(2026, 3, 1),
            new DateTime(2026, 3, 31),
            1000m,
            2000m,
            "Paid");

        var foundOrders = service.FindOrders(filter);

        foreach (var order in foundOrders)
        {
            Console.WriteLine(
                $"{order.CreatedAt:yyyy-MM-dd}: {order.Total} — {order.Status}");
        }

        var team = new Team();

        var firstEmployee =
            new Employee(1, "Олена");
        var secondEmployee =
            new Employee(2, "Андрій");

        team.AddEmployee(firstEmployee);
        team.AddEmployee(secondEmployee);

        foreach (var employee in team.Employees)
        {
            Console.WriteLine($"{employee.Id}: {employee.Name}");
        }

        team.RemoveEmployee(1);
    }
}
