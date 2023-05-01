export function initTUIEditor(elem, initialMarkdown) {
    const Editor = toastui.Editor;
    
    return new Editor({
        el: elem,
        height: '500px',
        initialEditType: 'wysiwyg',
        initialValue: initialMarkdown,
        previewStyle: 'tab',
    });
}

export function getMarkdown(editor) {
    return editor.getMarkdown();
}