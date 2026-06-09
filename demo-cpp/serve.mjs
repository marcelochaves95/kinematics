// Minimal static file server for the C++/WASM demo (correct .wasm MIME type).
import { createServer } from "node:http";
import { readFile } from "node:fs/promises";
import { extname, join, normalize } from "node:path";

const ROOT = new URL(".", import.meta.url).pathname;
const PORT = Number(process.env.PORT) || 8100;

const MIME = {
    ".html": "text/html; charset=utf-8",
    ".js": "text/javascript; charset=utf-8",
    ".mjs": "text/javascript; charset=utf-8",
    ".css": "text/css; charset=utf-8",
    ".wasm": "application/wasm",
    ".png": "image/png",
    ".json": "application/json",
};

createServer(async (req, res) => {
    try {
        let path = decodeURIComponent(new URL(req.url, "http://x").pathname);
        if (path === "/") path = "/index.html";
        const file = join(ROOT, normalize(path).replace(/^(\.\.[/\\])+/, ""));
        const body = await readFile(file);
        res.writeHead(200, { "Content-Type": MIME[extname(file)] || "application/octet-stream" });
        res.end(body);
    } catch {
        res.writeHead(404);
        res.end("not found");
    }
}).listen(PORT, () => console.log(`serving demo-cpp on http://localhost:${PORT}`));
