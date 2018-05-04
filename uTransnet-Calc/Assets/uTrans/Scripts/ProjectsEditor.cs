using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProjectsEditor
{
    List<Project> projects = new List<Project>();

    private Project _activeProject;
    public Project ActiveProject {
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

    public Project NewProject()
    {
        Debug.Log("newProject");
        ActiveProject = ScriptableObject.CreateInstance<Project>();
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
