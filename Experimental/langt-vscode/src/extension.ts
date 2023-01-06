'use strict';

import { workspace, Disposable, ExtensionContext } from 'vscode';
import { LanguageClient, LanguageClientOptions, SettingMonitor, ServerOptions, TransportKind, InitializeParams } from 'vscode-languageclient/node';
import { Trace } from 'vscode-jsonrpc';

// TODO: remove
const serverPath = String.raw`C:\Users\dylan\OneDrive\Desktop\Programming\langt\Experimental\langt-lsp\bin\Debug\net7.0\win-x64\langt-lsp.dll`

export function activate(context: ExtensionContext) {

    // The server is implemented in node
    let serverExe = 'dotnet';

    // If the extension is launched in debug mode then the debug server options are used
    // Otherwise the run options are used
    let serverOptions: ServerOptions = {
        run: { command: serverExe, args: [serverPath] },
        debug: { command: serverExe, args: [serverPath] }
    }

    // Options to control the language client
    let clientOptions: LanguageClientOptions = {
        // Register the server for plain text documents
        documentSelector: [
            {
                pattern: '**/*.lgt',
            }
        ],
        synchronize: {
            // Synchronize the setting section 'languageServerExample' to the server
            configurationSection: 'langtLanguageServer',
            fileEvents: workspace.createFileSystemWatcher('**/*.lgt')
        },
    }

    // Create the language client and start the client.
    const client = new LanguageClient('langtLanguageServer', 'Langt Language', serverOptions, clientOptions);
    client.setTrace(Trace.Verbose);
    
    client.start();
}