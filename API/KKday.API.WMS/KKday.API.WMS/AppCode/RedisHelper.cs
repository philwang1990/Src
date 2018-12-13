using System;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace KKday.API.WMS.AppCode
{
    public interface IRedisHelper {
        void SetRedis(string obj, string redisKey, int expireMinute);

        string getRedis(string redisKey);

        //IRedisHelper reChkConnect();
        ConnectionMultiplexer reChkConnect();
    }

    public class RedisHelper : IRedisHelper {

        public ConnectionMultiplexer kkrds { get; set; }
        public string redisConnectStr { get; private set; }

        public RedisHelper(IConfiguration configuration) {
            //redis 
            this.redisConnectStr = configuration["IP:REDIS_LAB"];
            kkrds = ConnectionMultiplexer.Connect(redisConnectStr);
        }

        //存到redis
        public void SetRedis(string redisKey, string obj, int expireMinute) {
            try {
                //kkredis  
                this.kkrds = reChkConnect();
                IDatabase db = kkrds.GetDatabase();
                db.StringSet(redisKey, obj, TimeSpan.FromMinutes(expireMinute));
            } catch (Exception ex) {
                Website.Instance.logger.Debug($"setRedis_error:" + ex.Message.ToString());
            }
        }

        public string getRedis(string redisKey) {
            try {
                //kkredis  
                this.kkrds = reChkConnect();
                IDatabase db = kkrds.GetDatabase();

                string obj = db.StringGet(redisKey);
                return obj;
            } catch (Exception ex) {
                Website.Instance.logger.Debug($"getRedis_error:" + ex.Message.ToString());
                return null;
            }
        }

        public ConnectionMultiplexer reChkConnect() {
            if (kkrds == null) {
                kkrds = ConnectionMultiplexer.Connect(this.redisConnectStr);
            }
            return kkrds;
        }

    }
}