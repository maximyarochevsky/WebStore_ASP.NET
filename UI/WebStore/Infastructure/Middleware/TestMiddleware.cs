namespace WebStore.Infastructure.Middleware
{
    public class TestMiddleware
    {
        private readonly RequestDelegate _Next;
        public TestMiddleware(RequestDelegate next)
        {
            _Next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            //обработка информации из Context.Request

            var proccessing_task = _Next(context);

            //выполнить какие-то действия параллельно асинхронно с остальной частью конвейера

            await proccessing_task;

            //доработка данных в Context.Response
           
        }
    }
}
