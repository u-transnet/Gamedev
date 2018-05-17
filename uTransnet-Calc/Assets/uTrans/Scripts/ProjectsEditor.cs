using System.Collections.Generic;
using UnityEngine;
using uTrans.Services;

namespace uTrans
{
    public class ProjectsEditor
    {
        List<Project> projects = new List<Project>();

        private Project _activeProject;

        public Project ActiveProject
        {
            get
            {
                return _activeProject;
            }
            set
            {
                if (_activeProject != null)
                {
                    _activeProject.Active = false;
                }
                _activeProject = value;
                if (_activeProject != null)
                {
                    _activeProject.Active = true;
                }
            }
        }

        public Project NewProject(int id = -1)
        {
            var project = ScriptableObject.CreateInstance<Project>();
            if(id < 0){
                Debug.Log("newProject");
                project.ProjectDTO = DataService.instance.ProjectDAO.New();
            } else {
                Debug.Log("Loading project " + id);
                project.ProjectDTO = DataService.instance.ProjectDAO.GetById(id);
            }
            ActiveProject = project;
            projects.Add(ActiveProject);

            return ActiveProject;
        }

        public void FinishProject()
        {
            ActiveProject = null;
        }

        public void DeleteProject()
        {
            if (ActiveProject != null)
            {
                ActiveProject.Delete();
                ActiveProject = null;
            }
        }

        public Project FindActiveProject(GameObject go)
        {
            Project _activeProject = null;
            foreach (Project proj in projects)
            {
                if (proj.Contains(go))
                {
                    _activeProject = proj;
                    break;
                }
            }

            ActiveProject = (_activeProject != null) ? _activeProject : ActiveProject;

            return _activeProject;
        }
    }
}
