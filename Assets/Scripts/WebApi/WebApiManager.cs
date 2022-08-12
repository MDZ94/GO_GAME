using Assets.Scripts.WebApi.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.WebApi
{
    public class WebApiManager : MonoBehaviour
    {
        private string bearerToken;
        private DateTime tokenExpiration;
        private string email;
        private string password;

        public void Clear() {
            bearerToken = "";
            tokenExpiration = DateTime.MinValue;
            email = "";
            password = "";
        }

        public DateTime TokenExpiration { get {
                return tokenExpiration;
            }
        }
        private readonly string apiBaseAddress = "https://fancymasters.com:44350/api";
        private HttpClient httpClient = new HttpClient();
        public WebApiManager() {
            httpClient.BaseAddress = new Uri(apiBaseAddress);
        }

        public async Task RegisterAsync(string email, string password, string nick, CancellationToken cancellationToken) {
            var relativeUrl = $"/Account/register";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<object>(httpClient);
            var userDTO = new UserDTO { email = email, password = password, nick = nick };

            await jsonHttpRequestHandler.Handle(HttpMethod.Post, relativeUrl, cancellationToken
                , payload: new StringContent(((JObject)JToken.FromObject(userDTO)).ToString(), Encoding.UTF8, "application/json"));
        }

        public async Task LoginAsync(string email, string password, CancellationToken cancellationToken) {
            try {
                var relativeUrl = $"/Account/login";
                var jsonHttpRequestHandler = new JsonHttpRequestHandler<TokenDTO>(httpClient);
                var userDTO = new LoginUserDTO { email = email, password = password};

                TokenDTO tokenDTO = await jsonHttpRequestHandler.Handle(HttpMethod.Post, relativeUrl, cancellationToken
                    , payload: new StringContent(((JObject)JToken.FromObject(userDTO)).ToString(), Encoding.UTF8, "application/json"));

                bearerToken = tokenDTO.token;
                tokenExpiration = GetExpFromToken(bearerToken);
                this.email = email;
                this.password = password;
            }
            catch (Exception ex) {
                if (ex.Message.StartsWith("401")) throw new ExceptionWithCode(2001, "Wrong login or password");
                else throw ex;
            }
        }

        public async Task<GameDTO> GameCreateAsync(string name, bool blackColor, BoardSize boardSize, int timeLimit, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Game/create";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<GameDTO>(httpClient);
            var gamePlayerDTO = new CreateGamePlayerDTO { blackColor = blackColor };
            var createGameDTO = new CreateGameDTO { name = name, boardSize = boardSize, timeLimit = timeLimit, gamePlayers = new List<CreateGamePlayerDTO> { gamePlayerDTO } };

            GameDTO gameDTO = await jsonHttpRequestHandler.Handle(HttpMethod.Post, relativeUrl, cancellationToken, headers: headerToken()
                , payload: new StringContent(((JObject)JToken.FromObject(createGameDTO)).ToString(), Encoding.UTF8, "application/json"));

            return gameDTO;
        }

        public async Task<GameDTO> GameGetAsync(int id, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Game/get/{id}";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<GameDTO>(httpClient);

            GameDTO gameDTO = await jsonHttpRequestHandler.Handle(HttpMethod.Get, relativeUrl, cancellationToken, headers: headerToken());

            return gameDTO;
        }

        public async Task<GamePlayerDTO> GameJoinAsync(int id, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Game/join/{id}";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<GamePlayerDTO>(httpClient);

            GamePlayerDTO gamePlayerDTO = await jsonHttpRequestHandler.Handle(HttpMethod.Post, relativeUrl, cancellationToken, headers: headerToken());

            return gamePlayerDTO;
        }

        public async Task GameSetReadyAsync(int gameId, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Game/setReady/{gameId}";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<object>(httpClient);

            await jsonHttpRequestHandler.Handle(HttpMethod.Post, relativeUrl, cancellationToken, headers: headerToken());
        }

        public async Task GameStartAsync(int gameId, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Game/startGame/{gameId}";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<object>(httpClient);

            await jsonHttpRequestHandler.Handle(HttpMethod.Post, relativeUrl, cancellationToken, headers: headerToken());
        }


        public async Task<MoveDTO> MoveAddAsync(int gameId, MoveType type, short posX, short posY, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Move/add/{gameId}";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<MoveDTO>(httpClient);
            var moveDTO = new CreateMoveDTO { PosX = posX, PosY = posY, Type = type};

            var result = await jsonHttpRequestHandler.Handle(HttpMethod.Post, relativeUrl, cancellationToken, headers: headerToken()
                , payload: new StringContent(((JObject)JToken.FromObject(moveDTO)).ToString(), Encoding.UTF8, "application/json"));

            return result;
        }

        public async Task<ScoreDTO> GetScore(int gameId, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Move/getScore/{gameId}";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<ScoreDTO>(httpClient);

            var scoreDTO = await jsonHttpRequestHandler.Handle(HttpMethod.Get, relativeUrl, cancellationToken, headers: headerToken());

            return scoreDTO;
        }

        public async Task<List<MoveDTO>> GetMoves(int gameId, int minMoveId, CancellationToken cancellationToken) {
            await CheckAndRenewToken(cancellationToken);
            var relativeUrl = $"/Move/get/{gameId}/{minMoveId}";
            var jsonHttpRequestHandler = new JsonHttpRequestHandler<List<MoveDTO>>(httpClient);

            var moves = await jsonHttpRequestHandler.Handle(HttpMethod.Get, relativeUrl, cancellationToken, headers: headerToken());

            return moves;
        }



        private DateTime GetExpFromToken(string token) {
            var parts = token.Split('.');
            var middle = parts[1];
            middle = middle.PadRight(middle.Length + (4 - middle.Length % 4) % 4, '=');
            var decoded = Convert.FromBase64String(middle);
            var part = Encoding.UTF8.GetString(decoded);
            var jwt = JObject.Parse(part);
            var expEpoch = jwt["exp"].Value<long>();
            var exp = DateTimeOffset.FromUnixTimeSeconds(expEpoch).DateTime.ToLocalTime();
            return exp;
        }


        private Dictionary<string, string> headerToken() {
            return new Dictionary<string, string> { { "Authorization", "Bearer " + bearerToken } };
        }

        private async Task CheckAndRenewToken(CancellationToken cancellationToken) {
            if(String.IsNullOrEmpty(email) || String.IsNullOrEmpty(password) || String.IsNullOrEmpty(bearerToken)) {
                throw new ExceptionWithCode(2002, "log in first");
            }
            if(String.IsNullOrEmpty(bearerToken) || (tokenExpiration - DateTime.Now).TotalMinutes < 2) {
                await LoginAsync(email, password, cancellationToken);
            }
            if(String.IsNullOrEmpty(bearerToken) || tokenExpiration < DateTime.Now){
                throw new ExceptionWithCode(2003, "Couldn't refresh token");
            }
        }
    }
}
