'use strict';
Object.defineProperty(exports, "__esModule", { value: true });
exports.activate = void 0;
const vscode_1 = require("vscode");
const node_1 = require("vscode-languageclient/node");
const vscode_jsonrpc_1 = require("vscode-jsonrpc");
// TODO: replace with stored path
const serverPath = String.raw `..\langt-lsp\bin\Debug\net7.0\win-x64\langt-lsp.dll`;
function activate(context) {
    const sp = context.extensionPath + '\\' + serverPath;
    // The server is implemented in node
    let serverExe = 'dotnet';
    // If the extension is launched in debug mode then the debug server options are used
    // Otherwise the run options are used
    let serverOptions = {
        run: { command: serverExe, args: [sp] },
        debug: { command: serverExe, args: [sp] }
    };
    // Options to control the language client
    let clientOptions = {
        // Register the server for plain text documents
        documentSelector: [
            {
                pattern: '**/*.lgt',
            }
        ],
        synchronize: {
            // Synchronize the setting section 'languageServerExample' to the server
            configurationSection: 'langtLanguageServer',
            fileEvents: vscode_1.workspace.createFileSystemWatcher('**/*.lgt')
        },
    };
    // Create the language client and start the client.
    const client = new node_1.LanguageClient('langtLanguageServer', 'Langt Language', serverOptions, clientOptions);
    client.setTrace(vscode_jsonrpc_1.Trace.Verbose);
    client.start();
}
exports.activate = activate;
//# sourceMappingURL=extension.js.map