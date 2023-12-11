using Models;
using Interface;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System;
using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

// using Task = Models.Task;

namespace services{

    public class TaskService : InterfaceTask
    {
     List<Task> ListTask {get;}

        // filePath
        private readonly int userId;

        private IWebHostEnvironment  webHost;
        private string filePath;
        public TaskService(IWebHostEnvironment webHost,IHttpContextAccessor httpContextAccessor)
            {
                this.webHost = webHost;
                this.userId = int.Parse(httpContextAccessor.HttpContext?.User?.FindFirst("UserId")?.Value);
                this.filePath = Path.Combine(webHost.ContentRootPath, "Data", "ListTask.json");
                using (var jsonFileTask = File.OpenText(filePath))
                {
                    ListTask = JsonSerializer.Deserialize<List<Task>>(jsonFileTask?.ReadToEnd(),
                    new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    });
                }
            }

        private void changeInFile()
        {
            File.WriteAllText(filePath, JsonSerializer.Serialize(ListTask));
        }

        // public void Add(long userId,Task task)

        public void Add(Task task)
        {
              task.Id = ListTask.Count()+ 1;
              if(task.AgentId== userId){
              task.AgentId = userId;
              ListTask.Add(task);
              changeInFile();
        }
        }
        // public void Delete(long userId,int id)

        public void Delete(int id)
        {
            var IdTask = GetById(id);
            if (IdTask is null)return;
            ListTask.Remove(IdTask);
            changeInFile();
                   
        }
        // public List<Task> Get(long userId)

        public List<Task> Get()
        {
            return ListTask.Where( t => t.AgentId == userId).ToList();
        }

        public Task GetById(int id)
        {
            return ListTask.FirstOrDefault(tl => tl.AgentId == userId && tl.Id == id);
        }
        // public void Update(long userId,Task task)

        public void Update(Task task)
        {
             var index = ListTask.FindIndex(tl => tl.AgentId == userId && tl.Id ==task.Id);
             if (index == -1)
                return;
            task.AgentId = userId;
            ListTask[index] = task;
            changeInFile();
        }
         public int Count()
        {
                return Get().Count();
        } 
    }


}