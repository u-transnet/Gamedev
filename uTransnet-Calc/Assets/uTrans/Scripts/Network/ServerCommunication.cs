using UnityEngine;

namespace uTrans.Network
{
    using System;
    using System.Collections.Generic;
    using uTrans.Data;
    using uTrans.Proto;


    public class ServerCommunication
    {
        public static void SendEnvelope(Request request, Action<Envelope> callback)
        {
            TCPClient tcpClient = new TCPClient();
            tcpClient.ConnectToTcpServer(() =>
            {
                tcpClient.SendMessage(request, response =>
                {
                    tcpClient.Disconnect();
                    callback(response);
                });
            });
        }

        public static void ListPresets(Action<List<PresetDTO>> callback)
        {
            Request request = new Request();
            request.Type = uTrans.Proto.Type.ListPresets;

            SendEnvelope(request, response =>
            {
                if (response.Type == uTrans.Proto.Type.ListPresets)
                {
                    List<PresetDTO> presetList = new List<PresetDTO>();
                    foreach (Preset item in response.Presets)
                    {
                        PresetDTO preset = new PresetDTO();
                        preset.Id = (int) item.Id;
                        preset.Name = item.Name;
                        presetList.Add(preset);
                    }
                    callback(presetList);
                }
            });
        }

        public static void GetPresetMaterials(int presetId, Action<List<PresetMaterialDTO>> callback)
        {
            Request request = new Request();
            request.Type = uTrans.Proto.Type.GetPresetMaterials;
            request.PresetId = CreateOptionalId(presetId);

            SendEnvelope(request, response =>
            {
                if (response.Type == uTrans.Proto.Type.GetPresetMaterials)
                {
                    List<PresetMaterialDTO> presetList = new List<PresetMaterialDTO>();
                    foreach (PresetMaterial item in response.PresetMaterials)
                    {
                        PresetMaterialDTO preset = new PresetMaterialDTO();
                        preset.MaterialId = (int) item.MaterialId;
                        preset.PresetId = presetId;
                        preset.Price = item.Price;
                        presetList.Add(preset);
                    }
                    callback(presetList);
                }
            });
        }


        public static void ListBaseObjects(Action<List<BaseObjectDTO>> callback)
        {
            Request request = new Request();
            request.Type = uTrans.Proto.Type.ListBaseObjects;

            SendEnvelope(request, response =>
            {
                if (response.Type == uTrans.Proto.Type.ListBaseObjects)
                {
                    List<BaseObjectDTO> presetList = new List<BaseObjectDTO>();
                    foreach (BaseObject item in response.BaseObjects)
                    {
                        BaseObjectDTO preset = new BaseObjectDTO();
                        preset.Id = (int) item.Id;
                        preset.Name = item.Name;
                        preset.MaxSize = item.MaxSize;
                        preset.MinSize = item.MinSize;
                        preset.Type = item.Type;
                        presetList.Add(preset);
                    }
                    callback(presetList);
                }
            });
        }


        public static void GetRequiredMaterialsForBaseObject(int baseObjectId, Action<List<BaseObjectMaterialDTO>> callback)
        {
            Request request = new Request();
            request.Type = uTrans.Proto.Type.GetRequiredMaterialsForBaseObject;
            request.PresetId = CreateOptionalId(baseObjectId);

            SendEnvelope(request, response =>
            {
                if (response.Type == uTrans.Proto.Type.GetRequiredMaterialsForBaseObject)
                {
                    List<BaseObjectMaterialDTO> presetList = new List<BaseObjectMaterialDTO>();
                    foreach (BaseObjectMaterial item in response.RequiredMaterials)
                    {
                        BaseObjectMaterialDTO preset = new BaseObjectMaterialDTO();
                        preset.BaseObjectId = baseObjectId;
                        preset.MaterialId = (int) item.MaterialId;
                        preset.Amount = item.Amount;
                        preset.OnExploitation = item.OnExploitation;
                        preset.UserEditable = item.UserEditable;
                        preset.GroupNumber = item.GroupNumber;
                        preset.NumberInGroup = item.NumberInGroup;
                        presetList.Add(preset);
                    }
                    callback(presetList);
                }
            });
        }


        public static void ListMaterials(Action<List<MaterialDTO>> callback)
        {
            Request request = new Request();
            request.Type = uTrans.Proto.Type.ListMaterials;

            SendEnvelope(request, response =>
            {
                if (response.Type == uTrans.Proto.Type.ListMaterials)
                {
                    List<MaterialDTO> presetList = new List<MaterialDTO>();
                    foreach (Material item in response.Materials)
                    {
                        MaterialDTO preset = new MaterialDTO();
                        preset.Id = (int) item.Id;
                        preset.Name = item.Name;
                        preset.Unit = (uTrans.Data.Unit) ((int) item.Unit); // Convert by id
                        preset.Income = item.Income;
                        presetList.Add(preset);
                    }
                    callback(presetList);
                }
            });
        }


        private static Request.Types.OptionalId CreateOptionalId(int presetId)
        {
            Request.Types.OptionalId optionalId = new Request.Types.OptionalId();
            optionalId.IsSet = true;
            optionalId.Id = presetId;
            return optionalId;
        }

    }
}