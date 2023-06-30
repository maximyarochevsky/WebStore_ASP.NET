﻿using System.Net.Http.Json;

namespace WebStore.WebAPI.Clients.Base;
public abstract class BaseClient
    {
    protected HttpClient Http { get; set; }
    protected string Address { get; }

    protected BaseClient(HttpClient Client, string Address)
    {
        Http = Client;
        this.Address = Address;
    }

    protected T? Get<T>(string url) => GetAsync<T>(url).Result;

    protected async Task<T?> GetAsync<T>(string url)
    {
        var response = await Http.GetAsync(url).ConfigureAwait(false); // в responce получаем ответ от сервера
        return await response
            .EnsureSuccessStatusCode()
            .Content
            .ReadFromJsonAsync<T>() // десериализация
            .ConfigureAwait(false);
    }

    protected HttpResponseMessage Post<T>(string url, T value) => PostAsync<T>(url, value).Result;

    protected async Task<HttpResponseMessage?> PostAsync<T>(string url, T value)
    {
        var response = await Http.PostAsJsonAsync(url, value).ConfigureAwait(false); // в responce получаем ответ от сервера
        return response.EnsureSuccessStatusCode();
    }

    protected HttpResponseMessage Put<T>(string url, T value) => PutAsync<T>(url, value).Result;

    protected async Task<HttpResponseMessage?> PutAsync<T>(string url, T value)
    {
        var response = await Http.PutAsJsonAsync(url, value).ConfigureAwait(false); // в responce получаем ответ от сервера
        return response.EnsureSuccessStatusCode();
    }

    protected HttpResponseMessage Delete(string url) => DeleteAsync(url).Result;

    protected async Task<HttpResponseMessage> DeleteAsync(string url)
    {
        var response = await Http.DeleteAsync(url).ConfigureAwait(false);
        return response;
    }
}

