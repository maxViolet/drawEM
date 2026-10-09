# drawEM

drawEM draws over the Windows desktop and plays sounds and screen effects from shortcuts during presentations and demonstrations.

## Language

**Effect**:
A short animation shown over the desktop on one monitor, started from an action slot.
_Avoid_: animation, overlay

**Built-in effect**:
An effect that ships inside drawEM, authored or selected by the drawEM team rather than the user, and identified by a stable effect ID. Each has one fixed placement.
_Avoid_: preset, template

**Placement**:
Where an effect appears on its monitor: **Monitor** (fills the whole monitor) or **Cursor** (fixed size, centered on the cursor point captured at start).
_Avoid_: position, anchor, mode

**Imported effect**:
An effect file the user adds to drawEM. Deferred beyond v4.
_Avoid_: custom effect
