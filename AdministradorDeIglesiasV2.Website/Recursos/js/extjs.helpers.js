/**
 * ExtJS Helper Functions
 */

Ext.form.TextArea.prototype.render = Ext.form.TextArea.prototype.render.createSequence(function () {
    if (Ext.isDefined(this.wrap)) {
        this.el.dom.wrap = this.wrap;
    }
});

function resetForm(formId) {
    var items = Ext.get(formId).query('.x-form-field');
    for (var i = 0; i < items.length; i++) {
        var item = items[i];
        if (Ext.getCmp(item.id)) {
            if (Ext.getCmp(item.id).reset) {
                Ext.getCmp(item.id).reset();
            }
        }
    }
}

function getAllRows(grid) {
    values = [];
    var data = grid.store.data.items;

    for (var i = 0; i < data.length; i++) {
        var record = grid.store.prepareRecord(data[i].data, data[i], {});
        values.push(record);
    }

    return values;
}

function getModifiedRows(grid) {
    var modified = grid.store.modified,
        deleted = grid.store.deleted,
        values = [];

    //We get the really "modified" rows
    for (var i = 0; i < modified.length; i++) {
        var record = grid.store.prepareRecord(modified[i].data, modified[i], {}),
            recordOriginal = modified[i].modified,
            reallyModified = false;

        if (!Ext.isEmptyObj(record)) {
            for (oValue in recordOriginal) { //We add the original values, only for modified rows; only if the original value and the new value are really different
                if (record[oValue] != recordOriginal[oValue]) {
                    record["o" + oValue] = recordOriginal[oValue];
                    reallyModified = true;
                }
            }

            if (reallyModified) {
                values.push(record);
            }
        }
    }

    //We get the "deleted" rows
    for (var i = 0; i < deleted.length; i++) {
        var record = grid.store.prepareRecord(deleted[i].data, deleted[i], {});
        record.deleted = true;
        values.push(record);
    }

    return values
}

function getComboTextForGrid(value, store, displayField) {
    var r = store.getById(value); //If r is undefined it might be that the store might not have the IDProperty defined

    if (Ext.isEmpty(r)) {
        return "";
    }

    return r.data[displayField];
}

function refreshTree(tree, directMethod, callback) {
    directMethod({
        success: function (result) {
            var nodes = eval(result);
            if (nodes.length > 0) {
                tree.initChildren(nodes);
            }
            else {
                tree.getRootNode().removeChildren();
            }

            if ((callback) && (typeof (callback) == 'function')) {
                callback();
            }
        }
    });
}

function toggleChecksOnTree(tree, isChecked) {
    if (tree) {
        var args = [isChecked];
        tree.root.suspendEvents();
        tree.root.cascade(function () {
            c = args[0];
            this.ui.toggleCheck(c);
            this.attributes.checked = c;
        }, null, args);
        tree.root.resumeEvents();
    }
}

function processOnEnter(id, destinationId) {
    el = Ext.get(id);
    if (el) {
        el.destinationEl = Ext.getCmp(destinationId);
        el.on('keydown', function (e) {
            if (e.keyCode == 13) {
                e.preventDefault();
                e.stopPropagation();
                e.stopEvent();

                if (this.destinationEl.type == 'button') {
                    this.destinationEl.focus();
                    this.destinationEl.fireEvent('click');
                }
                else {
                    this.destinationEl.clear();
                    this.destinationEl.focus();
                }
            }
        });
    }
}

function setElementState(element, enabled) {
    if (enabled) {
        Ext.getCmp(element).enable();
    }
    else {
        Ext.getCmp(element).disable();
        Ext.getCmp(element).setValue("");
    }
}

Ext.Renderers = {
    IsoDateSimple: function (value, meta, record) {
        if (value && value.trim().length >= 10) { // ISO: 2012-12-25T15:30:00Z
            return Ext.util.Format.date(new Date(parseInt(value.substring(0, 4)), parseInt(value.substring(5, 7) - 1), parseInt(value.substring(8, 10))), "d/M/y");
        }
        else {
            return "";
        }
    },
    IsoDateDayOfWeekOnly: function (value, meta, record) {
        if (value && value.trim().length >= 10) { // ISO: 2012-12-25T15:30:00Z
            return Ext.util.Format.date(new Date(parseInt(value.substring(0, 4)), parseInt(value.substring(5, 7) - 1), parseInt(value.substring(8, 10))), "l");
        }
        else {
            return "";
        }
    },
    Url: function (value, meta, record) {
        if (value && value.trim().length > 0) {
            return String.format('<a href="{0}" target="_blank">{0}</a>', (value.indexOf("http") != 0) ? "http://" + value : value);
        }
        else {
            return "";
        }
    },
    DetallesDeMiembro: function (value, meta, record) {
        var id = record.data.MiembroId || record.data.id || record.data.Id || record.data.ID;

        if (id > 0) {
            return String.format('<a href="#" onClick="javascript:ADMI.VerDetallesDeMiembro({1}); return false;">{0}</a>', value, id);
        }
        else {
            return value;
        }
    },
    DetallesDeCelula: function (value, meta, record) {
        var id = record.data.CelulaId || record.data.id || record.data.Id || record.data.ID;

        if (id > 0) {
            return String.format('<a href="#" onClick="javascript:ADMI.VerDetallesDeCelula({1}); return false;">{0}</a>', value, id);
        }
        else {
            return value;
        }
    },
    DetallesDeEnsayo: function (value, meta, record) {
        var id = record.data.AlabanzaEnsayoId || record.data.EnsayoId || record.data.id || record.data.Id || record.data.ID;

        if (id > 0) {
            return String.format('<a href="#" onClick="javascript:ADMI.VerDetallesDeEnsayo({1}); return false;">{0}</a>', value, id);
        }
        else {
            return value;
        }
    },
    DetallesDeEvento: function (value, meta, record) {
        var id = record.data.AlabanzaEventoId || record.data.EventoId || record.data.id || record.data.Id || record.data.ID;

        if (id > 0) {
            return String.format('<a href="#" onClick="javascript:ADMI.VerDetallesDeEvento({1}); return false;">{0}</a>', value, id);
        }
        else {
            return value;
        }
    },
    DetallesDeCancion: function (value, meta, record) {
        var id = record.data.AlabanzaCancionId || record.data.CancionId || record.data.id || record.data.Id || record.data.ID;

        if (id > 0) {
            return String.format('<a href="#" onClick="javascript:ADMI.VerDetallesDeCancion({1}); return false;">{0}</a>', value, id);
        }
        else {
            return value;
        }
    }
};