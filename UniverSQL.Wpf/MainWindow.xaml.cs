using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using UniverSQL.Core.Data;
using UniverSQL.Core.Services;

namespace UniverSQL.Wpf
{
    public partial class MainWindow : Window
    {
        private readonly DatabaseService _databaseService;

        public MainWindow()
        {
            InitializeComponent();

            // Setup Dependency Injection
            var services = new ServiceCollection();

            services.AddDbContext<SqlServerDbContext>(options =>
                options.UseInMemoryDatabase("SqlServerDb"));

            services.AddDbContext<Db2DbContext>(options =>
                options.UseInMemoryDatabase("Db2Db"));

            services.AddSingleton<DatabaseService>();

            var serviceProvider = services.BuildServiceProvider();

            // Seed Data
            using (var scope = serviceProvider.CreateScope())
            {
                var sqlContext = scope.ServiceProvider.GetRequiredService<SqlServerDbContext>();
                var db2Context = scope.ServiceProvider.GetRequiredService<Db2DbContext>();

                sqlContext.Database.EnsureCreated();
                db2Context.Database.EnsureCreated();
            }

            _databaseService = serviceProvider.GetRequiredService<DatabaseService>();
        }

        private async void LoadData_Click(object sender, RoutedEventArgs e)
        {
            var data = await _databaseService.GetJoinedDataAsync();
            ResultsGrid.ItemsSource = data.Select(d => new { Employee = d.EmployeeName, Department = d.DepartmentName }).ToList();
        }
    }
}
