window.Renderer = {
    _canvas: null,
    _ctx:    null,

    init(canvasId) {
        this._canvas = document.getElementById(canvasId);
        if (!this._canvas) return false;
        this._ctx = this._canvas.getContext('2d');
        return true;
    },

    clear(color) {
        this._ctx.fillStyle = color;
        this._ctx.fillRect(0, 0, this._canvas.width, this._canvas.height);
    },

    drawPolygon(points, fill, stroke, lineWidth) {
        if (!points || points.length < 2) return;
        const ctx = this._ctx;
        ctx.beginPath();
        ctx.moveTo(points[0].x, points[0].y);
        for (let i = 1; i < points.length; i++) ctx.lineTo(points[i].x, points[i].y);
        ctx.closePath();
        if (fill)   { ctx.fillStyle = fill; ctx.fill(); }
        if (stroke) { ctx.strokeStyle = stroke; ctx.lineWidth = lineWidth ?? 1.5; ctx.stroke(); }
    },

    drawCircle(x, y, r, fill, stroke, lineWidth) {
        const ctx = this._ctx;
        ctx.beginPath();
        ctx.arc(x, y, r, 0, Math.PI * 2);
        if (fill)   { ctx.fillStyle = fill; ctx.fill(); }
        if (stroke) { ctx.strokeStyle = stroke; ctx.lineWidth = lineWidth ?? 1; ctx.stroke(); }
    },

    drawLine(x1, y1, x2, y2, color, lineWidth) {
        const ctx = this._ctx;
        ctx.beginPath();
        ctx.moveTo(x1, y1);
        ctx.lineTo(x2, y2);
        ctx.strokeStyle = color ?? '#ffffff';
        ctx.lineWidth   = lineWidth ?? 1;
        ctx.stroke();
    },

    // ── Click / tap interaction ────────────────────────
    // Only listens to down events — the Blazor side applies
    // a one-shot impulse at the contact point.
    initInteraction(canvasId, dotNetRef) {
        const canvas = document.getElementById(canvasId);

        const pos = (e) => {
            const r   = canvas.getBoundingClientRect();
            const sx  = canvas.width  / r.width;
            const sy  = canvas.height / r.height;
            const src = e.touches ? e.touches[0] : e;
            return [
                (src.clientX - r.left) * sx,
                (src.clientY - r.top)  * sy
            ];
        };

        // Desktop
        canvas.addEventListener('mousedown', e => {
            const [x, y] = pos(e);
            dotNetRef.invokeMethodAsync('OnPointerDown', x, y);
        });

        // Mobile — preventDefault blocks page scroll while interacting
        canvas.addEventListener('touchstart', e => {
            e.preventDefault();
            const [x, y] = pos(e);
            dotNetRef.invokeMethodAsync('OnPointerDown', x, y);
        }, { passive: false });
    }
};

window.GameLoop = {
    _ref:   null,
    _rafId: null,

    start(dotNetRef) { this._ref = dotNetRef; this._tick(); },

    stop() {
        if (this._rafId) cancelAnimationFrame(this._rafId);
        this._rafId = null;
        this._ref   = null;
    },

    _tick() {
        this._ref.invokeMethodAsync('OnFrame').then(() => {
            this._rafId = requestAnimationFrame(() => this._tick());
        });
    }
};
