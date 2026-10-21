## graphify

This project has a knowledge graph at graphify-out/ with god nodes, community structure, and cross-file relationships.

Rules:
- For codebase questions, first use the `graphify` MCP server (`.mcp.json`): `query_graph` for questions, `shortest_path` for relationships, `get_node` and `get_neighbors` for focused concepts, `get_pr_impact` for PR scope. These return a scoped subgraph, usually much smaller than GRAPH_REPORT.md or raw grep output. The server needs `python -m pip install "graphifyy[mcp]==0.9.86"` in the `python` on PATH. If the MCP server is unavailable, fall back to `graphify query "<question>"`, `graphify path "<A>" "<B>"`, and `graphify explain "<concept>"`.
- If graphify-out/wiki/index.md exists, use it for broad navigation instead of raw source browsing.
- Read graphify-out/GRAPH_REPORT.md only for broad architecture review or when query/path/explain do not surface enough context.
- Do not run `graphify update .` after modifying code. The `Graphify weekly` workflow (`.github/workflows/graphify-weekly.yml`) rebuilds the graph every Monday and opens a PR. The graph can lag behind code by up to a week; confirm graph answers against the source when it matters.
- A committed graphify-out/graph.json is rewritten whole on every build, so a large diff to it in a `DOC: refresh graphify graph` pull request is expected. Do not flag it as churn. Other pull requests should not change graphify-out/.
