using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TelegramBotConstructor.Keyboards;

namespace TelegramBotConstructor.Iterfaces
{
    internal interface IStateHandler
    {
        void HandleWithoutResponce(Update update);
        Task<string> HandleWithResponceAsync(Update update);
    }
}
