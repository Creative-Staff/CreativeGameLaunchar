using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.IO;
using System;

[RequireComponent(typeof(RawImage), typeof(VideoPlayer), typeof(AudioSource))]
public class VideoPlayerOnUGui : MonoBehaviour
{
    [SerializeField] private RawImage image;
    [SerializeField] private VideoPlayer player;

    void Update()
    {
        //ビデオテクスチャを更新
        if (player.isPrepared) image.texture = player.texture;
    }
}