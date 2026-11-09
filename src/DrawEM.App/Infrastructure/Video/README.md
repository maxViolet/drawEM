# Video infrastructure

`Mp4VideoValidator` implements `IVideoValidator`. `Mp4Probe` reads the `ftyp`
box and the `moov` headers (sample entries, `avcC`, `esds`, media time, sample
count) and seeks past media data, so a file is classified in milliseconds
whatever its size. It decodes nothing: a stream whose SPS contradicts its
`avcC` passes validation and fails when playback opens it.

`MediaVideoPlayer` plays one file once through WPF `MediaPlayer`. Open and
decode errors arrive as `Failed` with the engine's HRESULT; after `Dispose` no
event is raised and the file is closed. `CreateDrawing` gives a `VideoDrawing`
for a surface.

Known engine behavior (S5-01): a file with valid headers and zeroed media data
opened and then neither ended nor failed within 10 s. A playback owner needs its
own no-progress limit.
