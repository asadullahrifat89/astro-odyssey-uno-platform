using System;
using System.Collections.Generic;
using System.Text;

namespace AstroOdyssey
{
    public interface IGameApiHelper
    {


    }

    public class GameApiHelper: IGameApiHelper
    {
        private readonly HttpRequestHelper _httpRequestHelper;

        public GameApiHelper(HttpRequestHelper httpRequestHelper)
        {
            _httpRequestHelper = httpRequestHelper;
        }
    }
}
