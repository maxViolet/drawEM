"""Generates the synthetic S4-03.1 probe Lottie files: a confetti-like Monitor load and a focus-ring Cursor load."""
import json
import random
import sys
from pathlib import Path

FPS = 60
out = Path(sys.argv[1])
rng = random.Random(4031)


def static(value):
    return {"a": 0, "k": value}


def animated(keys):
    frames = []
    for index, (time, value) in enumerate(keys):
        frame = {"t": time, "s": value if isinstance(value, list) else [value]}
        if index < len(keys) - 1:
            dims = len(frame["s"])
            frame["i"] = {"x": [0.4] * dims, "y": [1] * dims}
            frame["o"] = {"x": [0.6] * dims, "y": [0] * dims}
        frames.append(frame)
    return {"a": 1, "k": frames}


def transform(position, rotation=static(0), scale=static([100, 100, 100]), opacity=static(100)):
    return {"o": opacity, "r": rotation, "p": position, "a": static([0, 0, 0]), "s": scale}


def group(items):
    return {"ty": "gr", "it": items + [{
        "ty": "tr", "p": static([0, 0]), "a": static([0, 0]), "s": static([100, 100]),
        "r": static(0), "o": static(100)}]}


def layer(index, name, ks, shapes, op):
    return {"ddd": 0, "ind": index, "ty": 4, "nm": name, "sr": 1, "ks": ks, "ao": 0,
            "shapes": shapes, "ip": 0, "op": op, "st": 0, "bm": 0}


def document(name, width, height, op, layers):
    return {"v": "5.7.4", "fr": FPS, "ip": 0, "op": op, "w": width, "h": height, "nm": name,
            "ddd": 0, "assets": [], "layers": layers}


def confetti():
    width, height, op = 1920, 1080, 3 * FPS
    colors = [[0.96, 0.26, 0.21], [1, 0.76, 0.03], [0.3, 0.69, 0.31], [0.13, 0.59, 0.95], [0.61, 0.15, 0.69]]
    layers = []
    for index in range(1, 121):
        start = rng.randint(0, 40)
        x0 = rng.uniform(0, width)
        x1 = x0 + rng.uniform(-300, 300)
        flips = rng.choice([2, 3, 4])
        flip_keys = [(start + (op - start) * k / (flips * 2), [100 if k % 2 == 0 else -100, 100, 100])
                     for k in range(flips * 2 + 1)]
        ks = transform(
            animated([(start, [x0, -40, 0]), (op, [x1, height + 40, 0])]),
            animated([(start, 0), (op, rng.choice([-1, 1]) * rng.uniform(360, 1080))]),
            animated(flip_keys))
        shape = group([
            {"ty": "rc", "d": 1, "s": static([rng.uniform(12, 22), rng.uniform(6, 12)]), "p": static([0, 0]),
             "r": static(2)},
            {"ty": "fl", "c": static(rng.choice(colors) + [1]), "o": static(100), "r": 1}])
        layers.append(layer(index, f"piece {index}", ks, [shape], op))
    return document("probe confetti", width, height, op, layers)


def focus_ring():
    size, op = 240, 2 * FPS
    fade = op - FPS // 2
    rings = []
    for index, (diameter, width, delay) in enumerate([(200, 10, 0), (150, 6, 12)], start=1):
        ks = transform(
            static([size / 2, size / 2, 0]),
            scale=animated([(delay, [160, 160, 100]), (delay + 30, [100, 100, 100])]),
            opacity=animated([(delay, 0), (delay + 12, 100), (fade, 100), (op, 0)]))
        shape = group([
            {"ty": "el", "d": 1, "s": static([diameter, diameter]), "p": static([0, 0])},
            {"ty": "st", "c": static([1, 0.84, 0, 1]), "o": static(100), "w": static(width), "lc": 2, "lj": 2}])
        rings.append(layer(index, f"ring {index}", ks, [shape], op))
    return document("probe focus ring", size, size, op, rings)


out.mkdir(parents=True, exist_ok=True)
(out / "probe-confetti.json").write_text(json.dumps(confetti(), separators=(",", ":")), encoding="utf-8")
(out / "probe-focus-ring.json").write_text(json.dumps(focus_ring(), separators=(",", ":")), encoding="utf-8")
