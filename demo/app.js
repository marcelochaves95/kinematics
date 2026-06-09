// JS shell for the C++/WASM Kinematics demo.
// All physics AND scene construction run in C++ (kinematics.wasm); this file
// only loads the module, drives the frame loop, renders to canvas, and forwards
// pointer input as a radial impulse.

(function () {
    "use strict";

    // ── World / view constants (mirror the C# demo) ──────────────
    const WorldW = 20, WorldH = 14;
    const CanvasW = 800, CanvasH = 560;
    const Scale = CanvasW / WorldW;   // 40 px/m
    const Substeps = 4;
    const ImpulseRadius = 3;
    const MaxPoints = 64;
    const RippleDuration = 0.35;

    // ── Coordinate helpers ───────────────────────────────────────
    const toSX = wx => (wx + WorldW / 2) * Scale;
    const toSY = wy => wy * Scale;
    const toWX = sx => sx / Scale - WorldW / 2;
    const toWY = sy => sy / Scale;

    createKinematics().then(M => boot(M));

    function boot(M) {
        // ── C ABI bindings ───────────────────────────────────────
        const api = {
            createWorld:  M.cwrap("kn_world_create", "number", []),
            destroyWorld: M.cwrap("kn_world_destroy", null, ["number"]),
            step:         M.cwrap("kn_world_step", null, ["number", "number", "number"]),
            impulse:      M.cwrap("kn_world_apply_impulse", null,
                                  ["number", "number", "number", "number", "number"]),
            bodyCount:    M.cwrap("kn_world_body_count", "number", ["number"]),
            chainCount:   M.cwrap("kn_world_chain_count", "number", ["number"]),
            bodyIsStatic: M.cwrap("kn_body_is_static", "number", ["number", "number"]),
            bodyPoints:   M.cwrap("kn_body_point_count", "number", ["number", "number"]),
            getBodyPts:   M.cwrap("kn_body_get_points", "number",
                                  ["number", "number", "number", "number"]),
            chainPoints:  M.cwrap("kn_chain_point_count", "number", ["number", "number"]),
            getChainPts:  M.cwrap("kn_chain_get_points", "number",
                                  ["number", "number", "number", "number"]),
            demoSoftbody: M.cwrap("kn_demo_softbody", null, ["number"]),
            demoPressure: M.cwrap("kn_demo_pressure", null, ["number"]),
            demoChain:    M.cwrap("kn_demo_chain", null, ["number"]),
        };

        // Reusable scratch buffer for batch point read-back.
        const bufPtr = M._malloc(MaxPoints * 2 * 4);

        const scenes = [
            { name: "Soft Body", icon: "🟦",
              desc: "A spring-based box falls and bounces. Click to push it!",
              build: api.demoSoftbody, statLabel: "Pos Y", isChain: false },
            { name: "Pressure Body", icon: "🟣",
              desc: "A gas-inflated body bounces and deforms. Click to push it!",
              build: api.demoPressure, statLabel: "Pos Y", isChain: false },
            { name: "Chain", icon: "⛓️",
              desc: "A rope hanging under gravity. Click any link to flick it!",
              build: api.demoChain, statLabel: "Low Y", isChain: true },
        ];

        // ── Canvas ───────────────────────────────────────────────
        const canvas = document.getElementById("demo-canvas");
        const ctx = canvas.getContext("2d");

        // ── DOM refs ─────────────────────────────────────────────
        const el = {
            nav:   document.getElementById("nav-items"),
            name:  document.getElementById("scene-name"),
            desc:  document.getElementById("scene-desc"),
            frame: document.getElementById("stat-frame"),
            fps:   document.getElementById("stat-fps"),
            label: document.getElementById("stat-label"),
            value: document.getElementById("stat-value"),
            reset: document.getElementById("btn-reset"),
            slider: document.getElementById("impulse-slider"),
            impulseLabel: document.getElementById("impulse-label"),
        };

        // ── State ────────────────────────────────────────────────
        let world = 0;
        let active = null;
        let frame = 0;
        let fps = 0, fpsAccum = 0, fpsFrames = 0;
        let impulseStrength = 10;
        let ripple = null; // {wx, wy, age}

        // ── Scene management ─────────────────────────────────────
        function selectScene(scene) {
            if (world) api.destroyWorld(world);
            world = api.createWorld();
            scene.build(world); // C++ builds bodies/chain + arena + tuning
            active = scene;
            frame = 0; fps = 0; fpsAccum = 0; fpsFrames = 0; ripple = null;
            el.name.textContent = scene.name;
            el.desc.textContent = scene.desc;
            el.label.textContent = scene.statLabel;
            for (const b of el.nav.children) {
                b.classList.toggle("active", b.dataset.name === scene.name);
            }
        }

        // ── Build nav ────────────────────────────────────────────
        for (const scene of scenes) {
            const btn = document.createElement("button");
            btn.className = "nav-item";
            btn.dataset.name = scene.name;
            btn.innerHTML = `<span class="nav-icon">${scene.icon}</span>${scene.name}`;
            btn.addEventListener("click", () => selectScene(scene));
            el.nav.appendChild(btn);
        }
        el.reset.addEventListener("click", () => { if (active) selectScene(active); });
        el.slider.addEventListener("input", e => {
            impulseStrength = parseFloat(e.target.value);
            el.impulseLabel.textContent = impulseStrength.toFixed(0);
        });

        // ── Pointer input ────────────────────────────────────────
        function pointer(e) {
            const r = canvas.getBoundingClientRect();
            const src = e.touches ? e.touches[0] : e;
            const sx = (src.clientX - r.left) * (canvas.width / r.width);
            const sy = (src.clientY - r.top) * (canvas.height / r.height);
            const wx = toWX(sx), wy = toWY(sy);
            api.impulse(world, wx, wy, ImpulseRadius, impulseStrength);
            ripple = { wx, wy, age: 0 };
        }
        canvas.addEventListener("mousedown", pointer);
        canvas.addEventListener("touchstart", e => { e.preventDefault(); pointer(e); },
                                { passive: false });

        // ── Read-back helpers ────────────────────────────────────
        function readPoints(n) {
            const base = bufPtr >> 2; // float index
            const heap = M.HEAPF32;   // re-access (memory may have grown)
            const out = new Array(n);
            for (let i = 0; i < n; i++) {
                out[i] = { x: toSX(heap[base + 2 * i]), y: toSY(heap[base + 2 * i + 1]) };
            }
            return out;
        }

        // ── Draw primitives ──────────────────────────────────────
        function clear(color) { ctx.fillStyle = color; ctx.fillRect(0, 0, CanvasW, CanvasH); }
        function polygon(pts, fill, stroke, lw) {
            if (pts.length < 2) return;
            ctx.beginPath();
            ctx.moveTo(pts[0].x, pts[0].y);
            for (let i = 1; i < pts.length; i++) ctx.lineTo(pts[i].x, pts[i].y);
            ctx.closePath();
            if (fill) { ctx.fillStyle = fill; ctx.fill(); }
            if (stroke) { ctx.strokeStyle = stroke; ctx.lineWidth = lw; ctx.stroke(); }
        }
        function circle(x, y, rad, fill, stroke, lw) {
            ctx.beginPath();
            ctx.arc(x, y, rad, 0, Math.PI * 2);
            if (fill && fill !== "transparent") { ctx.fillStyle = fill; ctx.fill(); }
            if (stroke) { ctx.strokeStyle = stroke; ctx.lineWidth = lw; ctx.stroke(); }
        }
        function line(x1, y1, x2, y2, color, lw) {
            ctx.beginPath(); ctx.moveTo(x1, y1); ctx.lineTo(x2, y2);
            ctx.strokeStyle = color; ctx.lineWidth = lw; ctx.stroke();
        }

        // ── Render ───────────────────────────────────────────────
        function render() {
            clear("#0d0d1a");
            let statVal = 0;

            const nb = api.bodyCount(world);
            for (let b = 0; b < nb; b++) {
                const isStatic = api.bodyIsStatic(world, b) !== 0;
                const n = api.getBodyPts(world, b, bufPtr, MaxPoints);
                const pts = readPoints(n);
                polygon(pts, isStatic ? "#0a200a" : "#0d1f3a",
                             isStatic ? "#1e5a1e" : "#4a90e2", 1.5);
                if (!isStatic && !active.isChain) {
                    let sy = 0; for (const p of pts) sy += p.y;
                    statVal = toWY(sy / pts.length); // centroid Y in world units
                }
            }

            const nc = api.chainCount(world);
            for (let c = 0; c < nc; c++) {
                const n = api.getChainPts(world, c, bufPtr, MaxPoints);
                const pts = readPoints(n);
                for (let i = 0; i < n - 1; i++) {
                    line(pts[i].x, pts[i].y, pts[i + 1].x, pts[i + 1].y, "#c8a040", 2.5);
                }
                for (let i = 0; i < n; i++) {
                    const anchor = i === 0 || i === n - 1;
                    circle(pts[i].x, pts[i].y, anchor ? 7 : 4,
                           anchor ? "#a06020" : "#c8a040",
                           anchor ? "#e08040" : "#e0c060", 1);
                    if (active.isChain) {
                        const wy = toWY(pts[i].y);
                        if (wy > statVal) statVal = wy;
                    }
                }
            }

            if (ripple) {
                const t = ripple.age / RippleDuration;
                const rad = t * 80, opacity = 1 - t;
                circle(toSX(ripple.wx), toSY(ripple.wy), rad,
                       "transparent", `rgba(255,255,255,${opacity.toFixed(2)})`, 1.5);
            }

            el.value.textContent = statVal.toFixed(2);
        }

        // ── Frame loop ───────────────────────────────────────────
        const dt = 1 / 60;
        function tick() {
            api.step(world, dt, Substeps);
            frame++;

            fpsAccum += dt; fpsFrames++;
            if (fpsAccum >= 0.5) { fps = fpsFrames / fpsAccum; fpsAccum = 0; fpsFrames = 0; }
            if (ripple) { ripple.age += dt; if (ripple.age >= RippleDuration) ripple = null; }

            render();
            el.frame.textContent = frame;
            el.fps.textContent = fps.toFixed(0);
            requestAnimationFrame(tick);
        }

        selectScene(scenes[0]);
        requestAnimationFrame(tick);
    }
})();
