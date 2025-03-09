using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VNPAYSystem.Data.DBContext;
using VNPAYSystem.Service;

class Program
{

    static async Task Main()
    {
        // 1. Cấu hình DI
        var serviceProvider = ConfigureServices();

        // 2. Lấy instance của các service
        var userService = serviceProvider.GetRequiredService<IUserService>();
        var paymentService = serviceProvider.GetRequiredService<IPaymentService>();
        var walletService = serviceProvider.GetRequiredService<IWalletService>();
        var orderService = serviceProvider.GetRequiredService<IOrderService>();

        var orders = await orderService.GetAll();
        var payments = await paymentService.GetAll();
        var wallets = await walletService.GetAll();

        var loginResult = await userService.Login("a", "123");
        Console.WriteLine("=== Đăng nhập ===");
        Console.WriteLine("=== .... ===");
        if (loginResult != null)
        {
            Console.WriteLine("=== Thành công ===");
            Console.WriteLine($"ID: {loginResult.Id}, FullName: {loginResult.FullName}, Email: {loginResult.Email}, Phone: {loginResult.Phone}, CreatedAt: {loginResult.CreatedAt}");
        }


        Console.WriteLine("=== Danh sách đơn hàng ===");
        foreach (var order in orders)
        {
            Console.WriteLine($"ID: {order.Id}, Amount: {order.Amount}, Status: {order.Status}");
        }

        Console.WriteLine("\n=== Danh sách thanh toán ===");
        foreach (var payment in payments)
        {
            Console.WriteLine($"ID: {payment.Id}, Amount: {payment.Amount}, Method: {payment.BankCode}");
        }

        Console.WriteLine("\n=== Danh sách ví ===");
        foreach (var wallet in wallets)
        {
            Console.WriteLine($"ID: {wallet.UserId}, Balance: {wallet.Balance}");
        }

        Console.WriteLine("\nHoàn thành!");
    }



    public static ServiceProvider ConfigureServices()
    {
        var serviceCollection = new ServiceCollection();

        // Đọc file appsettings.json
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Lấy chuỗi kết nối
        string connectionString = configuration.GetConnectionString("DefaultConnection");

        // Cấu hình DbContext với SQL Server
        serviceCollection.AddDbContext<VNPAY_TestDBContext>(options =>
            options.UseSqlServer(connectionString));

        // Đăng ký Repository & Service
        serviceCollection.AddScoped<IUserService, UserService>();
        serviceCollection.AddScoped<IWalletService, WalletService>();
        serviceCollection.AddScoped<IOrderService, OrderService>();
        serviceCollection.AddScoped<IPaymentService, PaymentService>();

        return serviceCollection.BuildServiceProvider();
    }


}