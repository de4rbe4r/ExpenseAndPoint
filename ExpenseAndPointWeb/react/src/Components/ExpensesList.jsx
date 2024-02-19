import { React, useState, useEffect } from 'react';
import ListGroup from 'react-bootstrap/ListGroup';
import '../App.css';
import Row from 'react-bootstrap/Row';
import ExpenseForm from './ExpenseForm.jsx'
import ConfirmationForm from './ConfirmationForm'
import { ContextMenuTrigger, ContextMenu, ContextMenuItem } from 'rctx-contextmenu';



const ExpensesList = ({ expensesList, categoryList, userId, setIsDataUpdatedToParent, isDataUpdated, height, date }) => {

    const setIsShowEditForm = (data) => {
        setShowEditForm(data);
    }

    const setIsShowConfirmationForm = (data) => {
        setShowConfirmationForm(data);
    }

    const [showEditForm, setShowEditForm] = useState(false);
    const [showConfirmationForm, setShowConfirmationForm] = useState(false);

    const [expenseToEdit, setExpenseToEdit] = useState();
    const [expenseToDelete, setExpenseToDelete] = useState();
    const [categoryToDelete, setCategoryToDelete] = useState();

    useEffect(() => {
        if (expenseToDelete !== undefined) setCategoryToDelete(categoryList.find(c => c.id === expenseToDelete.categoryId));
    }, [expenseToDelete]);

    const openEditForm = (id) => {
        setExpenseToEdit(expensesList.find(e => e.id === id));
        setShowEditForm(true);
    }

    const setIsDataUpdated = (data) => {
        setIsDataUpdatedToParent(data);
    }

    const openConfirmationForm = (id) => {
        setExpenseToDelete(expensesList.find(e => e.id === id));
        setShowConfirmationForm(true);
    }

    if (categoryList.length === 0 || expensesList === null) return <img src="/loading.gif"></img>;


    return (
        <>
            <ExpenseForm action="Изменить" categoryList={categoryList} expenseToEdit={expenseToEdit}
                isShowForm={showEditForm} setIsShowForm={setIsShowEditForm} userId={userId} setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />

            <ConfirmationForm expense={expenseToDelete} category={categoryToDelete}
                isShowForm={showConfirmationForm} setIsShowForm={setIsShowConfirmationForm} setIsDataUpdated={setIsDataUpdated} isDataUpdated={isDataUpdated} />
            <ListGroup className="overflow-auto"
            style={{
                height: 0.85 * height,
                overflow: scroll
                }}>
                <ListGroup.Item variant="dark" key='Title' value='Title'>
                    <Row className="justify-content-md-center">
                        Список расходов {date}</Row>
                </ListGroup.Item>
                {
                    ((expensesList !== undefined && expensesList.count !== 0) ? (
                    expensesList.map((e, index) => (
                        <div key={`div-${e.id}`}>
                            <ContextMenuTrigger id={`menu-${e.id}`} key={`cmt-${e.id}`}>
                                <ListGroup.Item action variant="dark" key={e.id} value={e.id}>
                                    <Row className="justify-content-md-center" value={e.id}>
                                        {e.dateTime.replace('T', "").substr(10, 5)}
                                    </Row>
                                    <Row className="justify-content-md-center" value={e.id}>
                                        {(categoryList !== undefined) ? categoryList.find(c => c.id === e.categoryId).title : 'Без категории'}
                                    </Row>
                                    <Row className="justify-content-md-center" value={e.id}>
                                        {e.amount} &#x20bd;
                                    </Row>
                                </ListGroup.Item>
                            </ContextMenuTrigger>
                            <ContextMenu id={`menu-${e.id}`} key={`cm-${e.id}`}>
                                <ContextMenuItem onClick={() => openEditForm(e.id)}>Изменить</ContextMenuItem>
                                <ContextMenuItem onClick={() => openConfirmationForm(e.id)}>Удалить</ContextMenuItem>
                            </ContextMenu>
                        </div>
                    ))) : null)}
            </ListGroup>
        </>
    );
}

export default ExpensesList;
