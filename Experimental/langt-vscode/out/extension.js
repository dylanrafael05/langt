'use strict';
Object.defineProperty(exports, "__esModule", { value: true });
exports.activate = void 0;
const vscode_1 = require("vscode");
const vscode_languageclient_1 = require("vscode-languageclient");
const vscode_jsonrpc_1 = require("vscode-jsonrpc");
// TODO: remove!
const serverPath = String.raw `C:\Users\dylan\OneDrive\Desktop\Programming\langt\Experimental\langt-lsp\bin\Debug\net7.0\win-x64\langt-lsp.dll`;
function activate(context) {
    // The server is implemented in node
    let serverExe = 'dotnet';
    // If the extension is launched in debug mode then the debug server options are used
    // Otherwise the run options are used
    let serverOptions = {
        run: { command: serverExe, args: [serverPath] },
        debug: { command: serverExe, args: [serverPath] }
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
    const client = new vscode_languageclient_1.LanguageClient('langtLanguageServer', 'Langt Language', serverOptions, clientOptions);
    client.trace = vscode_jsonrpc_1.Trace.Verbose;
    let disposable = client.start();
    // Push the disposable to the context's subscriptions so that the
    // client can be deactivated on extension deactivation
    context.subscriptions.push(disposable);
}
exports.activate = activate;
//# sourceMappingURL=extension.js.map