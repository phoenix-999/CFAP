using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telerik.WinControls.UI.Localization;

namespace CFAP
{
    public class RussianRadGridLocalizationProvider : RadGridLocalizationProvider
    {
        public override string GetLocalizedString(string id)
        {
            switch (id)
            {
                case RadGridStringId.GroupingPanelDefaultMessage: return "Перетащите столбцы сюда, чтобы сгруппировать по этим столбцам";
                case RadGridStringId.GroupingPanelHeader: return "Группировать по:";

                case RadGridStringId.ConditionalFormattingPleaseSelectValidCellValue: return "Выберите действительное значение ячейки";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValue: return "Установите правильное значение ячейки";
                case RadGridStringId.ConditionalFormattingPleaseSetValidCellValues: return "Установите правильные значения ячеек";
                case RadGridStringId.ConditionalFormattingPleaseSetValidExpression: return "Установите правильное выражение";
                case RadGridStringId.ConditionalFormattingItem: return "Item";
                case RadGridStringId.ConditionalFormattingInvalidParameters: return "Неверные параметры";

                case RadGridStringId.SearchRowChooseColumns: return "Столбцы поиска";
                case RadGridStringId.SearchRowSearchFromCurrentPosition: return "Поиск строки с текущей позиции";
                case RadGridStringId.SearchRowMenuItemMasterTemplate: return "SearchRowMenuItemMasterTemplate";
                case RadGridStringId.SearchRowMenuItemChildTemplates: return "SearchRowMenuItemChildTemplates";
                case RadGridStringId.SearchRowMenuItemAllColumns: return "Искать во всех столбцах";
                case RadGridStringId.SearchRowTextBoxNullText: return "Поиск строки по отрывку текста";
                case RadGridStringId.SearchRowResultsOfLabel: return "SearchRowResultsOfLabel";
                case RadGridStringId.SearchRowMatchCase: return "Учитывать регистр";

                case RadGridStringId.FilterFunctionBetween: return "Между";
                case RadGridStringId.FilterFunctionContains: return "Содержит";
                case RadGridStringId.FilterFunctionDoesNotContain: return "Не содержит";
                case RadGridStringId.FilterFunctionEndsWith: return "Заканчиваеться на";
                case RadGridStringId.FilterFunctionEqualTo: return "Равно";
                case RadGridStringId.FilterFunctionGreaterThan: return "Больше";
                case RadGridStringId.FilterFunctionGreaterThanOrEqualTo: return "Больше или равно";
                case RadGridStringId.FilterFunctionIsEmpty: return "Пусто";
                case RadGridStringId.FilterFunctionIsNull: return "Не существует";
                case RadGridStringId.FilterFunctionLessThan: return "Меньше";
                case RadGridStringId.FilterFunctionLessThanOrEqualTo: return "Меньше или равно";
                case RadGridStringId.FilterFunctionNoFilter: return "Нет фильтров";
                case RadGridStringId.FilterFunctionNotBetween: return "Не между";
                case RadGridStringId.FilterFunctionNotEqualTo: return "Не равно";
                case RadGridStringId.FilterFunctionNotIsEmpty: return "Не пусто";
                case RadGridStringId.FilterFunctionNotIsNull: return "Существует";
                case RadGridStringId.FilterFunctionStartsWith: return "Начинается с";
                case RadGridStringId.FilterFunctionCustom: return "Клиентский";

                case RadGridStringId.FilterOperatorBetween: return "Между";
                case RadGridStringId.FilterOperatorContains: return "Содержит";
                case RadGridStringId.FilterOperatorDoesNotContain: return "Не содержит";
                case RadGridStringId.FilterOperatorEndsWith: return "Заканчивается на";
                case RadGridStringId.FilterOperatorEqualTo: return "Равно";
                case RadGridStringId.FilterOperatorGreaterThan: return "Больше";
                case RadGridStringId.FilterOperatorGreaterThanOrEqualTo: return "Больше или равно";
                case RadGridStringId.FilterOperatorIsEmpty: return "Пусто";
                case RadGridStringId.FilterOperatorIsNull: return "Не существует";
                case RadGridStringId.FilterOperatorLessThan: return "Меньше";
                case RadGridStringId.FilterOperatorLessThanOrEqualTo: return "Меньше или равно";
                case RadGridStringId.FilterOperatorNoFilter: return "Нет фильтров";
                case RadGridStringId.FilterOperatorNotBetween: return "Не между";
                case RadGridStringId.FilterOperatorNotEqualTo: return "Не равно";
                case RadGridStringId.FilterOperatorNotIsEmpty: return "Не пусто";
                case RadGridStringId.FilterOperatorNotIsNull: return "Существует";
                case RadGridStringId.FilterOperatorStartsWith: return "Начинается с";
                case RadGridStringId.FilterOperatorIsLike: return "Похожий на";
                case RadGridStringId.FilterOperatorNotIsLike: return "Не похожий на";
                case RadGridStringId.FilterOperatorIsContainedIn: return "Содержится в";
                case RadGridStringId.FilterOperatorNotIsContainedIn: return "Не содержится в";
                case RadGridStringId.FilterOperatorCustom: return "Клиентский";

                case RadGridStringId.CustomFilterMenuItem: return "Клиентский";
                case RadGridStringId.CustomFilterDialogCaption: return "RadGridView Filter Dialog [{0}]";
                case RadGridStringId.CustomFilterDialogLabel: return "Показать строки, где:";
                case RadGridStringId.CustomFilterDialogRbAnd: return "И";
                case RadGridStringId.CustomFilterDialogRbOr: return "ИЛИ";
                case RadGridStringId.CustomFilterDialogBtnOk: return "OK";
                case RadGridStringId.CustomFilterDialogBtnCancel: return "Отмена";
                case RadGridStringId.CustomFilterDialogCheckBoxNot: return "НЕ";
                case RadGridStringId.CustomFilterDialogTrue: return "ИСТИНА";
                case RadGridStringId.CustomFilterDialogFalse: return "ЛОЖЬ";

                case RadGridStringId.FilterMenuBlanks: return "Пусто";

                case RadGridStringId.FilterMenuAvailableFilters: return "Доступные фильтры";

                case RadGridStringId.FilterMenuSearchBoxText: return "Поиск...";

                case RadGridStringId.FilterMenuClearFilters: return "Очистить фильтр";

                case RadGridStringId.FilterMenuButtonOK: return "OK";
                case RadGridStringId.FilterMenuButtonCancel: return "Отмена";

                case RadGridStringId.FilterMenuSelectionAll: return "Все";
                case RadGridStringId.FilterMenuSelectionAllSearched: return "Все результаты поиска";
                case RadGridStringId.FilterMenuSelectionNull: return "Не существует";
                case RadGridStringId.FilterMenuSelectionNotNull: return "Существует";

                case RadGridStringId.FilterFunctionSelectedDates: return "Фильтр по конкретным датам:";
                case RadGridStringId.FilterFunctionToday: return "Сегодня";
                case RadGridStringId.FilterFunctionYesterday: return "Вчера";
                case RadGridStringId.FilterFunctionDuringLast7days: return "В течение последних 7 дней";

                case RadGridStringId.FilterLogicalOperatorAnd: return "И";
                case RadGridStringId.FilterLogicalOperatorOr: return "ИЛИ";
                case RadGridStringId.FilterCompositeNotOperator: return "НЕ";

                case RadGridStringId.DeleteRowMenuItem: return "Удалить строку";

                case RadGridStringId.SortAscendingMenuItem: return "Сортировать по возрастанию";
                case RadGridStringId.SortDescendingMenuItem: return "Сортировать по убыванию";
                case RadGridStringId.ClearSortingMenuItem: return "Очистить сортировку";

                case RadGridStringId.ConditionalFormattingMenuItem: return "Условное форматирование";
                case RadGridStringId.GroupByThisColumnMenuItem: return "Группировать по этому столбцу";
                case RadGridStringId.UngroupThisColumn: return "Разгруппировать этот столбец";

                //case RadGridStringId.ColumnChooserMenuItem: return "Column Chooser";

                case RadGridStringId.HideMenuItem: return "Скрыть столбец";
                case RadGridStringId.HideGroupMenuItem: return "Скрыть группу";
                case RadGridStringId.UnpinMenuItem: return "Открепить столбец";
                case RadGridStringId.UnpinRowMenuItem: return "Открепить строку";
                case RadGridStringId.PinMenuItem: return "Закрепленное состояние";
                case RadGridStringId.PinAtLeftMenuItem: return "Прикрепить слева";
                case RadGridStringId.PinAtRightMenuItem: return "Прикрепить справа";
                case RadGridStringId.PinAtBottomMenuItem: return "Прикрепить внизу";
                case RadGridStringId.PinAtTopMenuItem: return "Прикрепить сверху";

                case RadGridStringId.BestFitMenuItem: return "Наиболее подходящий";

                case RadGridStringId.PasteMenuItem: return "Вставить";
                case RadGridStringId.EditMenuItem: return "Редактировать";
                case RadGridStringId.ClearValueMenuItem: return "Очистить значение";
                case RadGridStringId.CopyMenuItem: return "Копировать";
                case RadGridStringId.CutMenuItem: return "Вырезать";
                case RadGridStringId.AddNewRowString: return "Нажмите здесь, чтобы добавить новую строку";
                case RadGridStringId.ConditionalFormattingSortAlphabetically: return "Сортировать столбцы по алфавиту";
                case RadGridStringId.ConditionalFormattingCaption: return "Менеджер правил условного форматирования";
                case RadGridStringId.ConditionalFormattingLblColumn: return "Форматировать только ячейки с";
                case RadGridStringId.ConditionalFormattingLblName: return "Название правила";
                
                //case RadGridStringId.ConditionalFormattingLblType: return "Cell value";
                //case RadGridStringId.ConditionalFormattingLblValue1: return "Value 1";
                //case RadGridStringId.ConditionalFormattingLblValue2: return "Value 2";
                //case RadGridStringId.ConditionalFormattingGrpConditions: return "Rules";
                //case RadGridStringId.ConditionalFormattingGrpProperties: return "Rule Properties";
                //case RadGridStringId.ConditionalFormattingChkApplyToRow: return "Apply this formatting to entire row";
                //case RadGridStringId.ConditionalFormattingChkApplyOnSelectedRows: return "Apply this formatting if the row is selected";
                //case RadGridStringId.ConditionalFormattingBtnAdd: return "Add new rule";
                //case RadGridStringId.ConditionalFormattingBtnRemove: return "Remove";
                //case RadGridStringId.ConditionalFormattingBtnOK: return "OK";
                //case RadGridStringId.ConditionalFormattingBtnCancel: return "Cancel";
                //case RadGridStringId.ConditionalFormattingBtnApply: return "Apply";
                //case RadGridStringId.ConditionalFormattingRuleAppliesOn: return "Rule applies to";
                //case RadGridStringId.ConditionalFormattingCondition: return "Condition";
                //case RadGridStringId.ConditionalFormattingExpression: return "Expression";
                //case RadGridStringId.ConditionalFormattingChooseOne: return "[Choose one]";
                //case RadGridStringId.ConditionalFormattingEqualsTo: return "equals to [Value1]";
                //case RadGridStringId.ConditionalFormattingIsNotEqualTo: return "is not equal to [Value1]";
                //case RadGridStringId.ConditionalFormattingStartsWith: return "starts with [Value1]";
                //case RadGridStringId.ConditionalFormattingEndsWith: return "ends with [Value1]";
                //case RadGridStringId.ConditionalFormattingContains: return "contains [Value1]";
                //case RadGridStringId.ConditionalFormattingDoesNotContain: return "does not contain [Value1]";
                //case RadGridStringId.ConditionalFormattingIsGreaterThan: return "is greater than [Value1]";
                //case RadGridStringId.ConditionalFormattingIsGreaterThanOrEqual: return "is greater than or equal [Value1]";
                //case RadGridStringId.ConditionalFormattingIsLessThan: return "is less than [Value1]";
                //case RadGridStringId.ConditionalFormattingIsLessThanOrEqual: return "is less than or equal to [Value1]";
                //case RadGridStringId.ConditionalFormattingIsBetween: return "is between [Value1] and [Value2]";
                //case RadGridStringId.ConditionalFormattingIsNotBetween: return "is not between [Value1] and [Value1]";
                //case RadGridStringId.ConditionalFormattingLblFormat: return "Format";
                //case RadGridStringId.ConditionalFormattingBtnExpression: return "Expression editor";
                //case RadGridStringId.ConditionalFormattingTextBoxExpression: return "Expression";
                //case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitive: return "CaseSensitive";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellBackColor: return "CellBackColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellForeColor: return "CellForeColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridEnabled: return "Enabled";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowBackColor: return "RowBackColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowForeColor: return "RowForeColor";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignment: return "RowTextAlignment";
                //case RadGridStringId.ConditionalFormattingPropertyGridTextAlignment: return "TextAlignment";
                //case RadGridStringId.ConditionalFormattingPropertyGridCaseSensitiveDescription: return "Determines whether case-sensitive comparisons will be made when evaluating string values.";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellBackColorDescription: return "Enter the background color to be used for the cell.";
                //case RadGridStringId.ConditionalFormattingPropertyGridCellForeColorDescription: return "Enter the foreground color to be used for the cell.";
                //case RadGridStringId.ConditionalFormattingPropertyGridEnabledDescription: return "Determines whether the condition is enabled (can be evaluated and applied).";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowBackColorDescription: return "Enter the background color to be used for the entire row.";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowForeColorDescription: return "Enter the foreground color to be used for the entire row.";
                //case RadGridStringId.ConditionalFormattingPropertyGridRowTextAlignmentDescription: return "Enter the alignment to be used for the cell values, when ApplyToRow is true.";
                //case RadGridStringId.ConditionalFormattingPropertyGridTextAlignmentDescription: return "Enter the alignment to be used for the cell values.";
                //case RadGridStringId.ColumnChooserFormCaption: return "Column Chooser";
                //case RadGridStringId.ColumnChooserFormMessage: return "Drag a column header from the\ngrid here to remove it from\nthe current view.";
                //case RadGridStringId.PagingPanelPagesLabel: return "Page";
                //case RadGridStringId.PagingPanelOfPagesLabel: return "of";
                //case RadGridStringId.NoDataText: return "No data to display";
                //case RadGridStringId.CompositeFilterFormErrorCaption: return "Filter Error";
                //case RadGridStringId.CompositeFilterFormInvalidFilter: return "The composite filter descriptor is not valid.";
                //case RadGridStringId.ExpressionMenuItem: return "Expression";
                //case RadGridStringId.ExpressionFormTitle: return "Expression Builder";
                //case RadGridStringId.ExpressionFormFunctions: return "Functions";
                //case RadGridStringId.ExpressionFormFunctionsText: return "Text";
                //case RadGridStringId.ExpressionFormFunctionsAggregate: return "Aggregate";
                //case RadGridStringId.ExpressionFormFunctionsDateTime: return "Date-Time";
                //case RadGridStringId.ExpressionFormFunctionsLogical: return "Logical";
                //case RadGridStringId.ExpressionFormFunctionsMath: return "Math";
                //case RadGridStringId.ExpressionFormFunctionsOther: return "Other";
                //case RadGridStringId.ExpressionFormOperators: return "Operators";
                //case RadGridStringId.ExpressionFormConstants: return "Constants";
                //case RadGridStringId.ExpressionFormFields: return "Fields";
                //case RadGridStringId.ExpressionFormDescription: return "Description";
                //case RadGridStringId.ExpressionFormResultPreview: return "Result preview";
                //case RadGridStringId.ExpressionFormTooltipPlus: return "Plus";
                //case RadGridStringId.ExpressionFormTooltipMinus: return "Minus";
                //case RadGridStringId.ExpressionFormTooltipMultiply: return "Multiply";
                //case RadGridStringId.ExpressionFormTooltipDivide: return "Divide";
                //case RadGridStringId.ExpressionFormTooltipModulo: return "Modulo";
                //case RadGridStringId.ExpressionFormTooltipEqual: return "Equal";
                //case RadGridStringId.ExpressionFormTooltipNotEqual: return "Not Equal";
                //case RadGridStringId.ExpressionFormTooltipLess: return "Less";
                //case RadGridStringId.ExpressionFormTooltipLessOrEqual: return "Less Or Equal";
                //case RadGridStringId.ExpressionFormTooltipGreaterOrEqual: return "Greater Or Equal";
                //case RadGridStringId.ExpressionFormTooltipGreater: return "Greater";
                //case RadGridStringId.ExpressionFormTooltipAnd: return "Logical \"AND\"";
                //case RadGridStringId.ExpressionFormTooltipOr: return "Logical \"OR\"";
                //case RadGridStringId.ExpressionFormTooltipNot: return "Logical \"NOT\"";
                //case RadGridStringId.ExpressionFormAndButton: return string.Empty; //if empty, default button image is used
                //case RadGridStringId.ExpressionFormOrButton: return string.Empty; //if empty, default button image is used
                //case RadGridStringId.ExpressionFormNotButton: return string.Empty; //if empty, default button image is used
                //case RadGridStringId.ExpressionFormOKButton: return "OK";
                //case RadGridStringId.ExpressionFormCancelButton: return "Cancel";
            }
            return base.GetLocalizedString(id);
        }
    }
}
