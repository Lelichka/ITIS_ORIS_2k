﻿namespace HttpServer.session;
using Microsoft.Extensions.Caching.Memory;

public class SessionManager
{
    private SessionManager()
    {
    }

    private static readonly Lazy<SessionManager> Lazy =
        new(() => new SessionManager());
    public static SessionManager Instance => Lazy.Value;
    
    private MemoryCache _cache = new(new MemoryCacheOptions());

    public void CreateSession(int id, int accountId, string login, DateTime createDateTime)
    {
        object key = id;
        var session = new Session(id, accountId, login, createDateTime);
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(120));
        
        _cache.Set(key, session, cacheEntryOptions);
    }


    public bool CheckSession(object key)
    {
        return _cache.TryGetValue(key, out _);
    }

    public Session? GetSessionInfo(object key)
    {
        _cache.TryGetValue(key, out Session? session);
        return session;
    }
}