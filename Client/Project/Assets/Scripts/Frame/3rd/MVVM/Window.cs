// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using Cysharp.Threading.Tasks;

// namespace Huge.MVVM
// {
//     public class Window : View
//     {
//         List<Section> m_SectionList = new List<Section>();
//         internal override async UniTask DestroyAsync()
//         {
//             foreach(Section section in m_SectionList)
//             {
//                 await section.DestroyAsync();
//             }
//             await base.DestroyAsync();
//         }

//         internal override void BeforeDestroy()
//         {
//             foreach(Section section in m_SectionList)
//             {
//                 section.BeforeDestroy();
//             }
//             base.BeforeDestroy();
//         }
        
//         protected async UniTask<Section> AddSection(ViewInfo sectionInfo, GameObject parent, params object[] args)
//         {
//             Section section = Activator.CreateInstance(sectionInfo.Section) as Section;
//             m_SectionList.Add(section);
//             try
//             {
//                 await section.CreateAsync(sectionInfo);
//                 section.AfterCreate(args);
//                 section.SetParent(parent);
//                 section.SetActive(true);
//                 return section;
//             }
//             catch (Exception ex)
//             {
//                 Huge.Debug.LogError($"UI: init {sectionInfo.Section.Name} error: {ex.Message}.");
//                 m_SectionList.Remove(section);
//                 return null;
//             }
//         }

//         protected async UniTask<Section> AddSection(ViewInfo sectionInfo, GameObject root, GameObject parent, params object[] args)
//         {
//             Section section = Activator.CreateInstance(sectionInfo.Section) as Section;
//             m_SectionList.Add(section);
//             try
//             {
//                 await section.CreateAsync(sectionInfo, root);
//                 section.AfterCreate(args);
//                 section.SetParent(parent);
//                 section.SetActive(true);
//                 return section;
//             }
//             catch (Exception ex)
//             {
//                 Huge.Debug.LogError($"UI: init {section.GetType().Name} error: {ex.Message}.");
//                 m_SectionList.Remove(section);
//                 return null;
//             }
//         }

//         protected void RemoveSection(Section section)
//         {
//             if (section != null && m_SectionList.Contains(section))
//             {
//                 m_SectionList.Remove(section);
//                 try
//                 {
//                     section.BeforeDestroy();
//                     section.DestroyAsync().Forget();
//                 }
//                 catch (Exception ex)
//                 {
//                     Huge.Debug.LogError($"UI: remove {section.GetType().Name} error: {ex.Message}.");
//                 }
//             }
//         }

//         protected bool HasSection(SectionInfo sectionInfo) 
//         {
//             return (GetSection(sectionInfo) != null);
//         }

//         protected Section GetSection(SectionInfo sectionInfo)
//         {
//             foreach(var section in m_SectionList)
//             {
//                 if (section.GetSectionInfo() == sectionInfo)
//                 {
//                     return section;
//                 }
//             }
//             return null;
//         }

//         protected List<Section> GetSections(SectionInfo sectionInfo)
//         {
//             List<Section> sections = new List<Section>();
//             foreach(var section in m_SectionList)
//             {
//                 if (section.GetSectionInfo() == sectionInfo)
//                 {
//                     sections.Add(section);
//                 }
//             }
//             return sections;
//         }
//     }
// }