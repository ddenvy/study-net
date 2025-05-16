using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Networking.Tests
{
    public class NetworkingTests
    {
        // Тест 1: Проверка работы с TcpListener и TcpClient
        [Fact]
        public async Task TcpListener_ShouldAcceptConnection()
        {
            // Arrange
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            var message = "Test message";

            // Act
            var clientTask = Task.Run(async () =>
            {
                using var client = new TcpClient();
                await client.ConnectAsync(IPAddress.Loopback, port);
                var stream = client.GetStream();
                var bytes = Encoding.UTF8.GetBytes(message);
                await stream.WriteAsync(bytes, 0, bytes.Length);
            });

            using var server = await listener.AcceptTcpClientAsync();
            var serverStream = server.GetStream();
            var buffer = new byte[1024];
            var bytesRead = await serverStream.ReadAsync(buffer, 0, buffer.Length);
            var receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Assert
            Assert.Equal(message, receivedMessage);
            listener.Stop();
        }

        // Тест 2: Проверка работы с UdpClient
        [Fact]
        public async Task UdpClient_ShouldSendAndReceive()
        {
            // Arrange
            var server = new UdpClient(0);
            var client = new UdpClient();
            var serverEndPoint = (IPEndPoint)server.Client.LocalEndPoint;
            var message = "Test message";

            // Act
            var bytes = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(bytes, bytes.Length, serverEndPoint);
            var result = await server.ReceiveAsync();
            var receivedMessage = Encoding.UTF8.GetString(result.Buffer);

            // Assert
            Assert.Equal(message, receivedMessage);
        }

        // Тест 3: Проверка работы с HttpClient
        [Fact]
        public async Task HttpClient_ShouldMakeRequest()
        {
            // Arrange
            using var client = new HttpClient();
            var message = "Test message";

            // Act
            var content = new StringContent(message);
            var response = await client.PostAsync("http://localhost:5000/test", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.True(response.IsSuccessStatusCode);
        }

        // Тест 4: Проверка работы с WebClient
        [Fact]
        public async Task WebClient_ShouldDownloadData()
        {
            // Arrange
            using var client = new WebClient();
            var url = "http://localhost:5000/test";

            // Act
            var data = await client.DownloadDataTaskAsync(url);

            // Assert
            Assert.True(data.Length > 0);
        }

        // Тест 5: Проверка работы с Socket
        [Fact]
        public async Task Socket_ShouldConnect()
        {
            // Arrange
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(new IPEndPoint(IPAddress.Loopback, 0));
            listener.Listen(1);
            var port = ((IPEndPoint)listener.LocalEndPoint).Port;
            var message = "Test message";

            // Act
            var clientTask = Task.Run(async () =>
            {
                using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                await client.ConnectAsync(IPAddress.Loopback, port);
                var bytes = Encoding.UTF8.GetBytes(message);
                await client.SendAsync(bytes, SocketFlags.None);
            });

            using var server = await listener.AcceptAsync();
            var buffer = new byte[1024];
            var bytesRead = await server.ReceiveAsync(buffer, SocketFlags.None);
            var receivedMessage = Encoding.UTF8.GetString(buffer, 0, bytesRead);

            // Assert
            Assert.Equal(message, receivedMessage);
        }

        // Тест 6: Проверка работы с Dns
        [Fact]
        public void Dns_ShouldResolveHostName()
        {
            // Arrange
            var hostName = "localhost";

            // Act
            var addresses = Dns.GetHostAddresses(hostName);

            // Assert
            Assert.Contains(IPAddress.Loopback, addresses);
        }

        // Тест 7: Проверка работы с IPAddress
        [Fact]
        public void IPAddress_ShouldParse()
        {
            // Arrange
            var ipString = "127.0.0.1";

            // Act
            var ipAddress = IPAddress.Parse(ipString);

            // Assert
            Assert.Equal(IPAddress.Loopback, ipAddress);
        }

        // Тест 8: Проверка работы с IPEndPoint
        [Fact]
        public void IPEndPoint_ShouldCreate()
        {
            // Arrange
            var ipAddress = IPAddress.Loopback;
            var port = 8080;

            // Act
            var endPoint = new IPEndPoint(ipAddress, port);

            // Assert
            Assert.Equal(ipAddress, endPoint.Address);
            Assert.Equal(port, endPoint.Port);
        }

        // Тест 9: Проверка работы с NetworkStream
        [Fact]
        public async Task NetworkStream_ShouldTransferData()
        {
            // Arrange
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            var port = ((IPEndPoint)listener.LocalEndpoint).Port;
            var message = "Test message";

            // Act
            var clientTask = Task.Run(async () =>
            {
                using var client = new TcpClient();
                await client.ConnectAsync(IPAddress.Loopback, port);
                using var stream = client.GetStream();
                var writer = new StreamWriter(stream);
                await writer.WriteLineAsync(message);
                await writer.FlushAsync();
            });

            using var server = await listener.AcceptTcpClientAsync();
            using var stream = server.GetStream();
            var reader = new StreamReader(stream);
            var receivedMessage = await reader.ReadLineAsync();

            // Assert
            Assert.Equal(message, receivedMessage);
            listener.Stop();
        }

        // Тест 10: Проверка работы с HttpListener
        [Fact]
        public async Task HttpListener_ShouldHandleRequest()
        {
            // Arrange
            var listener = new HttpListener();
            listener.Prefixes.Add("http://localhost:5000/");
            listener.Start();
            var message = "Test response";

            // Act
            var contextTask = listener.GetContextAsync();
            using var client = new HttpClient();
            var responseTask = client.GetAsync("http://localhost:5000/");

            var context = await contextTask;
            var response = context.Response;
            var buffer = Encoding.UTF8.GetBytes(message);
            await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);
            response.Close();

            var clientResponse = await responseTask;
            var responseContent = await clientResponse.Content.ReadAsStringAsync();

            // Assert
            Assert.Equal(message, responseContent);
            listener.Stop();
        }
    }
} 